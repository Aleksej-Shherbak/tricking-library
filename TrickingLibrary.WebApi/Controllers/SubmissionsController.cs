using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.BackgroundServices;
using TrickingLibrary.WebApi.RequestModels;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _env;

        public SubmissionsController(ApplicationDbContext applicationDbContext, IWebHostEnvironment env)
        {
            _applicationDbContext = applicationDbContext;
            _env = env;
        }

        [HttpGet()]
        public Task<Submission[]> All() => _applicationDbContext.Submissions
            .Where(x => !x.IsDeleted && x.IsVideoProcessed)
            .ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Submission> Get(int id) => _applicationDbContext.Submissions.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost()]
        public async Task<ActionResult<Submission>> Create([FromForm] SubmissionFormModel submission, 
            [FromServices] Channel<ProcessVideoMessage> channel)
        {
            if (!await _applicationDbContext.Tricks.AnyAsync(x => x.Id == submission.TrickId))
            {
                return BadRequest("Category not found!");
            }

            var mime = Path.GetExtension(submission.Video.FileName);
            var fileName = string.Concat($"temp_{DateTime.Now.Ticks}", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await submission.Video.CopyToAsync(fileStream);
            
            var newSubmission = new Submission
            {
                Name = submission.Name,
                Description = submission.Description,
                Video = fileName,
                TrickId = submission.TrickId
            };

            // TODO: validate video.
            await _applicationDbContext.AddAsync(newSubmission);

            // TODO: Maybe it worth to create db entity after processing the video?
            await _applicationDbContext.SaveChangesAsync();
            await channel.Writer.WriteAsync(new ProcessVideoMessage
            {
                SubmissionId = newSubmission.Id,
                Input = fileName,
                Output = $"converted_{DateTime.Now.Ticks}.mp4",
            });
            
            return Ok(newSubmission);
        }

        [HttpPut()]
        public IActionResult Update([FromBody] Submission submission)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.Requests.SubmissionsController;

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
            .Where(x => !x.IsDeleted)
            .ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Submission> Get(int id) => _applicationDbContext.Submissions.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost()]
        public async Task<ActionResult<Submission>> Create([FromForm] CreateSubmissionRequest submission)
        {
            if (!await _applicationDbContext.Tricks.AnyAsync(x => x.Id == submission.TrickId))
            {
                return BadRequest("Category not found!");
            }

            var mime = submission.Video.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await submission.Video.CopyToAsync(fileStream);
            var newTrick = new Submission
            {
                Name = submission.Name,
                Video = fileName,
                TrickId = submission.TrickId
            };

            await _applicationDbContext.AddAsync(newTrick);
            await _applicationDbContext.SaveChangesAsync();
            return Ok(newTrick);
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
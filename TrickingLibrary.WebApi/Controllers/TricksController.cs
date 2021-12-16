using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.Requests.TricksController;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TricksController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _env;

        public TricksController(ApplicationDbContext applicationDbContext, IWebHostEnvironment env)
        {
            _applicationDbContext = applicationDbContext;
            _env = env;
        }

        [HttpGet()]
        public Task<Trick[]> All() => _applicationDbContext.Tricks
            .Where(x => !x.IsDeleted)
            .ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Trick> Get(int id) => _applicationDbContext.Tricks.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost()]
        public async Task<ActionResult<Trick>> Create([FromForm] CreateTrickRequest trick)
        {
            if (!await _applicationDbContext.Categories.AnyAsync(x => x.Id == trick.CategoryId))
            {
                return BadRequest("Category not found!");
            }

            var mime = trick.Video.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await trick.Video.CopyToAsync(fileStream);
            var newTrick = new Trick
            {
                Name = trick.Name,
                Video = fileName,
                CategoryId = trick.CategoryId
            };

            await _applicationDbContext.AddAsync(newTrick);
            await _applicationDbContext.SaveChangesAsync();
            return Ok(newTrick);
        }

        [HttpPut()]
        public IActionResult Update([FromBody] Trick trick)
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
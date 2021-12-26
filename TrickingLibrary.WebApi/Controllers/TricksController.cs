using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.Mapping;
using TrickingLibrary.WebApi.RequestModels;
using TrickingLibrary.WebApi.ResponseModels;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TricksController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TricksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{trickId}/submissions")]
        public async Task<SubmissionResponseModel[]> GetSubmissionsForTrick(string trickId) => 
            (await _context.Submissions
                .Include(x => x.Video)
                .Where(x => x.TrickId == trickId && !x.IsDeleted)
                .ToArrayAsync())
            .Select(x => x.MapToViewModel())
            .ToArray();
        
        [HttpGet()]
        public async Task<TrickResponseModel[]> Index() => 
            (await _context.Tricks
                .Include(x => x.TrickCategories)
                .Include(x => x.Prerequisites)
                .Include(x => x.Progressions)
                .ToArrayAsync())
            .Select(x => x.MapToViewModel())
            .ToArray();

        [HttpPost()]
        public async Task<ActionResult<TrickResponseModel>> Create([FromBody] TrickFormModel model)
        {
            // Create slug
            var id = Regex.Replace(model.Name.Trim(), @"\s+", " ")
                .Replace(" ", "-")
                .ToLowerInvariant();
            // check if exists
            if (await _context.Tricks.AnyAsync(x => x.Id == id))
            {
                return BadRequest("Trick already exists");
            }
            
            var trick = new Trick
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Difficulty = model.Difficulty,
                TrickCategories = model.Categories.Select(x => new TrickCategory
                {
                    CategoryId = x
                }).ToList()
            };

            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return trick.MapToViewModel();
        }
        
        [HttpPut()]
        public async Task<ActionResult<TrickResponseModel>> Update(Trick trick)
        {
            if (string.IsNullOrWhiteSpace(trick.Id))
            {
                return NotFound();
            }

            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return Ok(trick.MapToViewModel());
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrickResponseModel>> Delete(string trickId)
        {
           var trick = await _context.Tricks.FirstOrDefaultAsync(x => x.Id == trickId);
            if (trick == null)
            {
                return NotFound();
            }

            trick.IsDeleted = true;
            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return Ok(trick.MapToViewModel());
        }
    }
}
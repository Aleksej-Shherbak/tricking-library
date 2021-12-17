using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;

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
        public Task<Submission[]> GetSubmissionsForTrick(string trickId) => _context.Submissions
            .Where(x => x.TrickId == trickId && !x.IsDeleted).ToArrayAsync();
        
        [HttpGet()]
        public Task<Trick[]> Index() => _context.Tricks.ToArrayAsync();

        [HttpPost()]
        public async Task<ActionResult<Trick>> Create(Trick trick)
        {
            // Create slug
            trick.Id = Regex.Replace(trick.Name.Trim(), @"\s+", " ")
                .Replace(" ", "-")
                .ToLowerInvariant();
            // check if exists
            if (await _context.Tricks.AnyAsync(x => x.Id == trick.Id))
            {
                return BadRequest("Trick already exists");
            }

            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return trick;
        }
        
        [HttpPut()]
        public async Task<ActionResult<Trick>> Update(Trick trick)
        {
            if (string.IsNullOrWhiteSpace(trick.Id))
            {
                return NotFound();
            }

            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return Ok(trick);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trick>> Delete(string trickId)
        {
           var trick = await _context.Tricks.FirstOrDefaultAsync(x => x.Id == trickId);
            if (trick == null)
            {
                return NotFound();
            }

            trick.IsDeleted = true;
            await _context.Tricks.AddAsync(trick);
            await _context.SaveChangesAsync();
            return Ok(trick);
        }
    }
}
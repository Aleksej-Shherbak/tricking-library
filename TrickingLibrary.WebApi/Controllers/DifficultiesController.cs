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
    public class DifficultiesController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DifficultiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public Task<Difficulty[]> Index() => _context.Difficulties.ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Difficulty> Get(string id) => _context.Difficulties.FirstOrDefaultAsync(x => x.Id == id);

        [HttpGet("{id}/tricks")]
        public Task<Trick[]> ListDifficultyTricks(string id) => _context.Tricks
            .Where(x => x.Difficulty == id && !x.IsDeleted)
            .ToArrayAsync();
        
        [HttpPost()]
        public async Task<ActionResult<Difficulty>> Create(Difficulty difficulty)
        {
            // Create slug
            difficulty.Id = Regex.Replace(difficulty.Name.Trim(), @"\s+", " ")
                .Replace(" ", "-")
                .ToLowerInvariant();
            // check if exists
            if (await _context.Tricks.AnyAsync(x => x.Id == difficulty.Id))
            {
                return BadRequest("Difficulty already exists");
            }

            await _context.Difficulties.AddAsync(difficulty);
            await _context.SaveChangesAsync();
            return difficulty;
        }
        
       
    }
}
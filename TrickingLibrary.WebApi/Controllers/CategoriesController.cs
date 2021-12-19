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
    public class CategoriesController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public Task<Category[]> Index() => _context.Categories.ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Category> Get(string id) => _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        [HttpGet("{id}/tricks")]
        public Task<Trick[]> ListCategoryTricks(string id) => _context.TrickCategories
            .Include(x => x.Trick)
            .Where(x => x.TrickId == id && !x.Trick.IsDeleted)
            .Select(x => x.Trick)
            .ToArrayAsync();
        
        [HttpPost()]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            // Create slug
            category.Id = Regex.Replace(category.Name.Trim(), @"\s+", " ")
                .Replace(" ", "-")
                .ToLowerInvariant();
            // check if exists
            if (await _context.Categories.AnyAsync(x => x.Id == category.Id))
            {
                return BadRequest("Category already exists");
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
        
       
    }
}
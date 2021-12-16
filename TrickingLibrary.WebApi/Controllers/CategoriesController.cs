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

        [HttpGet("{categoryId}/tricks")]
        public Task<Trick[]> GetTricksInCategory(string categoryId) => _context.Tricks
            .Where(x => x.CategoryId == categoryId && !x.IsDeleted).ToArrayAsync();
        
        [HttpGet()]
        public Task<Category[]> Index() => _context.Categories.ToArrayAsync();

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
        
        [HttpPut()]
        public async Task<ActionResult<Category>> Update(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                return NotFound();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(string categoryId)
        {
           var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
    }
}
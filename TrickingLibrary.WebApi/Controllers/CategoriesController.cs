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

        [HttpPost()]
        public async Task<Category> Create(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
        
        [HttpPut()]
        public async Task<ActionResult<Category>> Update(Category category)
        {
            if (category.Id == 0)
            {
                return NotFound();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(int categoryId)
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
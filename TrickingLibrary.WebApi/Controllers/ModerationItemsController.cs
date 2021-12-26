using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities.Moderation;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationItemsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModerationItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public Task<ModerationItem[]> Index() => _context.ModerationItems.ToArrayAsync();

        [HttpGet("{id}")]
        public Task<ModerationItem> Get(int id) => _context.ModerationItems.FirstOrDefaultAsync(x => x.Id == id);
    }
}
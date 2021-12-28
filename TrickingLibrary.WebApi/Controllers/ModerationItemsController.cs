using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.Entities.Moderation;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationItemsController: ControllerBase
    {
        // TODO move to a service.
        private readonly Regex _commentRegexp = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)", RegexOptions.Compiled);
        
        private readonly ApplicationDbContext _context;

        public ModerationItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public Task<ModerationItem[]> Index() => _context.ModerationItems.ToArrayAsync();

        [HttpGet("{id}")]
        public Task<ModerationItem> Get(int id) => _context.ModerationItems.FirstOrDefaultAsync(x => x.Id == id);

        [HttpGet("{id}/comments")]
        public async Task<Comment[]> GetComments(int id) =>
            (await _context.ModerationItems
                .Include(x => x.Comments)
                .Where(x => x.Id == id)
                .Select(x => x.Comments)
                .FirstOrDefaultAsync())
            .ToArray();

        [HttpPost("{id}/comment")]
        public async Task<ActionResult<Comment>> Comment(int id, [FromBody] Comment comment)
        {
            var moderationItem = await _context.ModerationItems.FirstOrDefaultAsync(x => x.Id == id);

            if (moderationItem == null)
            {
                return NotFound();
            }
            // TODO move to services
            comment.HtmlContent = comment.Content;
            var matches = _commentRegexp.Matches(comment.Content);
            foreach (Match match in matches)
            {
                var tag = match.Groups["tag"].Value;
                comment.HtmlContent = comment.HtmlContent
                    .Replace(tag, $"<a href=\"tag-user-link\">{tag}</a>");
            }

            moderationItem.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }
    }
}
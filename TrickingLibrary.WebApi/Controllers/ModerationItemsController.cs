using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.Entities.Moderation;
using TrickingLibrary.WebApi.Mapping;
using TrickingLibrary.WebApi.ResponseModels;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationItemsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CommentService _commentService;

        public ModerationItemsController(ApplicationDbContext context, CommentService commentService)
        {
            _context = context;
            _commentService = commentService;
        }

        [HttpGet()]
        public Task<ModerationItem[]> Index() => _context.ModerationItems.ToArrayAsync();

        [HttpGet("{id}")]
        public Task<ModerationItem> Get(int id) => _context.ModerationItems.FirstOrDefaultAsync(x => x.Id == id);

        [HttpGet("{id}/comments")]
        public async Task<CommentResponseModel[]> GetComments(int id) =>
            (await _context.Comments.Where(x => x.ModerationItemId == id).ToArrayAsync())
            .Select(x => x.MapToViewModel())
            .ToArray();

        [HttpPost("{id}/comment")]
        public async Task<ActionResult<CommentResponseModel>> Comment(int id, [FromBody]Comment comment)
        {
            var moderationItem = await _context.ModerationItems.FirstOrDefaultAsync(x => x.Id == id);

            if (moderationItem == null)
            {
                return NotFound();
            }

            comment = _commentService.CreateCommentHtmlContent(comment);

            moderationItem.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment.MapToViewModel());
        }
    }
}
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.Mapping;
using TrickingLibrary.WebApi.ResponseModels;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CommentService _commentService;

        public CommentsController(ApplicationDbContext context, CommentService commentService)
        {
            _context = context;
            _commentService = commentService;
        }

        [HttpGet("{id}/replies")]
        public async Task<CommentResponseModel[]> GetReplies(int id) =>
            (await _context.Comments.Where(x => x.ParentId == id).ToArrayAsync())
            .Select(x => x.MapToViewModel())
            .ToArray();

        [HttpPost("{id}/replies")]
        public async Task<ActionResult<CommentResponseModel>> Reply(int id, [FromBody] Comment reply)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            
            if (comment == null)
            {
                return NotFound();
            }

            reply = _commentService.CreateCommentHtmlContent(reply);
            comment.Replies.Add(reply);
            await _context.SaveChangesAsync();
            return Ok(reply.MapToViewModel());
        }


    }
}
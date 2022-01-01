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
            if (!await _context.ModerationItems.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }

            comment = _commentService.CreateCommentHtmlContent(comment);
            comment.ModerationItemId = id;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment.MapToViewModel());
        }
        
        [HttpGet("{id}/reviews")]
        public Task<Review[]> GetReviews(int id) =>
            _context.Reviews.Where(x => x.ModerationItemId == id).ToArrayAsync();

        [HttpPost("{id}/reviews")]
        public async Task<ActionResult<Review>> Review(int id, [FromBody]Review review)
        {
            if (!await _context.ModerationItems.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }

            review.ModerationItemId = id;
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }
    }
}
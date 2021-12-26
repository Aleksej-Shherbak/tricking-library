using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.WebApi.Mapping;
using TrickingLibrary.WebApi.RequestModels;
using TrickingLibrary.WebApi.ResponseModels;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SubmissionService _submissionService;

        public SubmissionsController(ApplicationDbContext applicationDbContext, SubmissionService submissionService)
        {
            _applicationDbContext = applicationDbContext;
            _submissionService = submissionService;
        }

        [HttpGet()]
        public async Task<SubmissionResponseModel[]> All() => (await _applicationDbContext.Submissions
                .Where(x => !x.IsDeleted && x.IsVideoProcessed)
                .Include(x => x.Video)
                .ToArrayAsync())
            .Select(x => x.MapToViewModel())
            .ToArray();
            

        [HttpGet("{id}")]
        public async Task<SubmissionResponseModel> Get(int id) => (await _applicationDbContext.Submissions
            .Include(x => x.Video)
            .FirstOrDefaultAsync(x => x.Id == id))
            .MapToViewModel();

        [HttpPost()]
        public Task Create([FromForm] SubmissionRequestModel submission) =>
            _submissionService.CreateSubmissionAsync(submission);
        
    }
}
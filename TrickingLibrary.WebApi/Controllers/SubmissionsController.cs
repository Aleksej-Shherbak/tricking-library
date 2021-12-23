using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.RequestModels;
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
        public Task<Submission[]> All() => _applicationDbContext.Submissions
            .Where(x => !x.IsDeleted && x.IsVideoProcessed)
            .ToArrayAsync();

        [HttpGet("{id}")]
        public Task<Submission> Get(int id) => _applicationDbContext.Submissions.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost()]
        public Task Create([FromForm] SubmissionFormModel submission) =>
            _submissionService.CreateSubmissionAsync(submission);
        
    }
}
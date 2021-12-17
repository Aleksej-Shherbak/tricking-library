using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TrickingLibrary.WebApi.Requests.SubmissionsController
{
    public class CreateSubmissionRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Video { get; set; }
        [Required]
        public string TrickId { get; set; }
    }
}
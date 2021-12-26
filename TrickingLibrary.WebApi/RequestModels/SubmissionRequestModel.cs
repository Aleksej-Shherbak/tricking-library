using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TrickingLibrary.WebApi.RequestModels
{
    public class SubmissionRequestModel
    {
        public string Name { get; set; }
        [Required]
        public IFormFile Video { get; set; }
        [Required]
        public string TrickId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
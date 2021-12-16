using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class VideosController: ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public VideosController(IWebHostEnvironment env)
        {
            _env = env;
        }
        
        [HttpGet("{video}")]
        public IActionResult GetVideo([Required] string video)
        { 
            var path = Path.Combine(_env.WebRootPath, video);
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            return new FileStreamResult(new FileStream(path, FileMode.Open), "video/*");
        }
    }
}
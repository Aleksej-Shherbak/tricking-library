using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.WebApi.BackgroundServices;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class VideosController: ControllerBase
    {
        private readonly VideoManager _videoManager;

        public VideosController(VideoManager videoManager)
        {
            _videoManager = videoManager;
        }
        
        [HttpGet("{video}")]
        public IActionResult GetVideo([Required] string video)
        { 
            var path = _videoManager.GetVideoPath(video);
            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
            {
                return NotFound();
            }

            return new FileStreamResult(new FileStream(path, FileMode.Open), "video/*");
        }
    }
}
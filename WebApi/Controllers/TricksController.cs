using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Requests.TricksController;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TricksController : ControllerBase
    {
        private readonly TrickyStore _trickyStore;
        private readonly IWebHostEnvironment _env;

        public TricksController(TrickyStore trickyStore, IWebHostEnvironment env)
        {
            _trickyStore = trickyStore;
            _env = env;
        }

        [HttpGet()]
        public IEnumerable<Trick> All() => _trickyStore.All;

        [HttpGet("{id}")]
        public Trick Get(int id) => _trickyStore.All.FirstOrDefault(x => x.Id == id);

        [HttpPost()]
        public async Task<Trick> Create([FromForm] CreateTrickRequest trick)
        {
            var mime = trick.Video.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await trick.Video.CopyToAsync(fileStream);
            var newTrick = new Trick
            {
                Name = trick.Name,
                VideoName = fileName
            }; 
            
            _trickyStore.Add(newTrick);
            return newTrick;
        }

        [HttpPut()]
        public IActionResult Update([FromBody] Trick trick)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}
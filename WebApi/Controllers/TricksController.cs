using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TricksController: ControllerBase
    {
        private readonly TrickyStore _trickyStore;

        public TricksController(TrickyStore trickyStore)
        {
            _trickyStore = trickyStore;
        }

        [HttpGet()]
        public IEnumerable<Trick> All() => _trickyStore.All;
        
        [HttpGet("{id}")]
        public Trick Get(int id) => _trickyStore.All.FirstOrDefault(x => x.Id == id);
        
        [HttpPost()]
        public Trick Create(Trick trick)
        {
             _trickyStore.Add(trick);
             return trick;
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
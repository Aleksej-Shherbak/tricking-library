using System.Collections.Generic;

namespace TrickingLibrary.WebApi.RequestModels
{
    public class TrickFormModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
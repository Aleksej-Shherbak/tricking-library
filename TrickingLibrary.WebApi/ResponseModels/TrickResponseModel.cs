using System.Collections.Generic;

namespace TrickingLibrary.WebApi.ResponseModels
{
    public record TrickResponseModel
    {
        public string Id { get; init; }
        public string Description { get; init; }
        public string Difficulty { get; init; }
        public IEnumerable<string> Categories { get; init; }
        public string Name { get; init; }
        public IEnumerable<string> Prerequisites { get; set; }
        public IEnumerable<string> Progression { get; set; }
    }
}
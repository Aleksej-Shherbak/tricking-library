using System.Collections.Generic;

namespace TrickingLibrary.Entities
{
    public class Difficulty : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Trick> Tricks { get; set; }
    }
}
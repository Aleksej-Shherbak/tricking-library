using System.Collections.Generic;

namespace TrickingLibrary.Entities
{
    public class Trick: BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public IList<TrickRelationship> Prerequisites { get; set; }
        public IList<TrickRelationship> Progressions { get; set; }
        public IList<TrickCategory> TrickCategories { get; set; }
    }
}
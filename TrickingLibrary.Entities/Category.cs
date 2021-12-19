using System.Collections.Generic;

namespace TrickingLibrary.Entities
{
    public class Category : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TrickCategory> Tricks { get; set; }
    }
}
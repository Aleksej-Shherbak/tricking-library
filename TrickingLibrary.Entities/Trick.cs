using System.Collections;

namespace TrickingLibrary.Entities
{
    public class Trick: BaseEntity<string>
    {
        public string Name { get; set; }
        /*public string Description { get; set; }
        public string DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }*/
    }

    /*public class Difficulty : BaseEntity<string>
    {
        public string Description { get; set; }
        public IList<Trick> Categories { get; set; }
    }*/
}
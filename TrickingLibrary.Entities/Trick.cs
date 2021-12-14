namespace TrickingLibrary.Entities
{
    public class Trick: BaseEntity
    {
        public string Name { get; set; }
        public string Video { get; set; }
        public int CategoryId { get; set; }
    }
}
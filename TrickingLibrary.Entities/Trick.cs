namespace TrickingLibrary.Entities
{
    public class Trick: BaseEntity<int>
    {
        public string Name { get; set; }
        public string Video { get; set; }
        public string CategoryId { get; set; }
    }
}
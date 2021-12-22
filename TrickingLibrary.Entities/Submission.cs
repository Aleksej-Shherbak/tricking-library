namespace TrickingLibrary.Entities
{
    public class Submission: BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Video { get; set; }
        public string TrickId { get; set; }
        public bool IsVideoProcessed { get; set; }
    }
}
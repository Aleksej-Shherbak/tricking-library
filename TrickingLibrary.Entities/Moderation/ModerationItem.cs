namespace TrickingLibrary.Entities.Moderation
{
    public class ModerationItem: BaseEntity<int>
    {
        public string Target { get; set; }
        public string Type { get; set; }
    }
}
namespace TrickingLibrary.Entities
{
    public class Comment: BaseEntity<int>
    {
        public string Content { get; set; }
        public string HtmlContent { get; set; }
    }
}
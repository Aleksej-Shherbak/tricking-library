namespace TrickingLibrary.WebApi.ResponseModels
{
    public class CommentResponseModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }
    }
}
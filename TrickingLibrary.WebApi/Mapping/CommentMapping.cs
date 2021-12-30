using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.ResponseModels;

namespace TrickingLibrary.WebApi.Mapping
{
    public static class CommentMapping
    {
        public static CommentResponseModel MapToViewModel(this Comment source)
        {
            return new CommentResponseModel
            {
                Id = source.Id,
                Content = source.Content,
                HtmlContent = source.HtmlContent,
                ParentId = source.ParentId,
            };
        }
    }
}
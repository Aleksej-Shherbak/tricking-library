using System.Text.RegularExpressions;
using TrickingLibrary.Entities;

namespace TrickingLibrary.WebApi.Services
{
    public class CommentService
    {
        private readonly Regex _commentRegexp = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)", RegexOptions.Compiled);

        public Comment CreateCommentHtmlContent(Comment comment)
        {
            comment.HtmlContent = comment.Content;
            var matches = _commentRegexp.Matches(comment.Content);
            foreach (Match match in matches)
            {
                var tag = match.Groups["tag"].Value;
                comment.HtmlContent = comment.HtmlContent
                    .Replace(tag, $"<a href=\"tag-user-link\">{tag}</a>");
            }

            return comment;
        }
    }
}
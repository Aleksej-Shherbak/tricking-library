using System.Collections.Generic;

namespace TrickingLibrary.Entities.Moderation
{
    public class ModerationItem: BaseEntity<int>
    {
        public string Target { get; set; }
        public string Type { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}
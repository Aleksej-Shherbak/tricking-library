namespace TrickingLibrary.Entities.Moderation
{
    public class Review: BaseEntity<int>
    {
        /// <summary>
        /// Explanation for your decision of the review.
        /// </summary>
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
        public int ModerationItemId { get; set; }
        public ModerationItem ModerationItem { get; set; }
    }
}
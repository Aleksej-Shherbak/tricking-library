namespace TrickingLibrary.Entities
{
    public class Video: BaseEntity<int>
    {
        public string FileName { get; set; }

        public string ThumbnailFileName { get; set; }
    }
}
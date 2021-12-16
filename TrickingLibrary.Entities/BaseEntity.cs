namespace TrickingLibrary.Entities
{
    public abstract class BaseEntity<KType>
    {
        public KType Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
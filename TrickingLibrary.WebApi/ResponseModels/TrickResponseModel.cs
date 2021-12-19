namespace TrickingLibrary.WebApi.ResponseModels
{
    public record TrickResponseModel
    {
        public string Id { get; init; }
        public string Description { get; init; }
        public string Difficulty { get; init; }
        public string[] Categories { get; init; }
        public string Name { get; init; }
    }
}
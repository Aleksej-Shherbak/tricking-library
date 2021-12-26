using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.ResponseModels;

namespace TrickingLibrary.WebApi.Mapping
{
    public static class SubmissionMapping
    {
        public static SubmissionResponseModel MapToViewModel(this Submission source)
        {
            return new SubmissionResponseModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Video = source.Video?.FileName,
                Thumbnail = source.Video?.ThumbnailFileName,
            };
        }
    }
}
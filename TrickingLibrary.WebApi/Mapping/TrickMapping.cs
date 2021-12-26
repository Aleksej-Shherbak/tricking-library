using System.Linq;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.ResponseModels;

namespace TrickingLibrary.WebApi.Mapping
{
    public static class TrickMapping
    {
        public static TrickResponseModel MapToViewModel(this Trick source)
        {
            return new TrickResponseModel
            {
                Id = source.Id,
                Description = source.Description,
                Name = source.Name,
                Difficulty = source.Difficulty,
                Prerequisites = source.Prerequisites.Select(x => x.PrerequisiteId),
                Progression = source.Progressions.Select(x => x.ProgressionId),
                Categories = source.TrickCategories?.Select(x => x.CategoryId)
            };
        }
    }
}
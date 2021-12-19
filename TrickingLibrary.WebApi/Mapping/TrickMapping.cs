using System.Linq;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.ResponseModels;

namespace TrickingLibrary.WebApi.Mapping
{
    public static class TrickMapping
    {
        public static TrickResponseModel MapToViewModels(this Trick source)
        {
            return new TrickResponseModel
            {
                Id = source.Id,
                Description = source.Description,
                Name = source.Name,
                Difficulty = source.Difficulty,
                Categories = source.TrickCategories?.Select(x => x.CategoryId).ToArray()
            };
        }
    }
}
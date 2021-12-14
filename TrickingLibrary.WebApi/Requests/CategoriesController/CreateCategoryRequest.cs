using System.ComponentModel.DataAnnotations;

namespace TrickingLibrary.WebApi.Requests.CategoriesController
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
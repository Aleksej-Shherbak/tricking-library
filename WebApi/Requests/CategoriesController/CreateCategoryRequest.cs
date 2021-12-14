using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests.CategoriesController
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
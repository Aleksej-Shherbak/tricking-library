using Microsoft.AspNetCore.Mvc;
using WebApi.Requests.CategoriesController;
using WebApi.Responses.CategoriesController;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController: ControllerBase
    {
        [HttpPost()]
        public CreateCategoryResponse Create(CreateCategoryRequest request)
        {
            return new CreateCategoryResponse();
        }
    }
}
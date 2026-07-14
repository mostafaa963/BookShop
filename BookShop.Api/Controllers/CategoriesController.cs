using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<GenResponse<CategoryIndexDto>>> GetCountAllCategory()
        {
            var response = await _categoryService.GetCountAllCategoryAsync();
            return Ok(new GenResponse<CategoryIndexDto>
            {
                Success = true,
                Message = "Get All Count Successfully",
                Data = response
            });
        }

    }
}

using Ecommerce.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController(IProductService productService, ICategoryService categoryService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }
    }
}

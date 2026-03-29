using Ecommerce.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(IProductService productService, ICategoryService categoryService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Product = _productService.Id });
        }
    }
}

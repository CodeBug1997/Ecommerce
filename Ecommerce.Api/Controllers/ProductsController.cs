using Ecommerce.Api.Services.Category;
using Ecommerce.Api.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        public ProductsController(ProductService productService, CategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Product = _productService.Id, Category = _categoryService.GetProductId });
        }
    }
}

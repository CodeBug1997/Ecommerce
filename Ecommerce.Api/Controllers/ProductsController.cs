using Ecommerce.Api.Services.Category;
using Ecommerce.Api.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Product = _productService.Id, Category = _categoryService.GetProductId() });
        }
    }
}

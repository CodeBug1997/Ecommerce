using Ecommerce.Api.Services.Product;

namespace Ecommerce.Api.Services.Category
{
    public class CategoryService
    {
        private readonly ProductService _productService; 
        public CategoryService(ProductService productService)
        {
            _productService = productService;
        }

        public Guid GetProductId => _productService.Id;
    }
}

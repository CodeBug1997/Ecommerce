using Ecommerce.Repository.Entities;
using Ecommerce.Repository.Repositories;
using Ecommerce.Service.Dtos.ProductDtos;
using Microsoft.VisualBasic;

namespace Ecommerce.Service.Services
{
    public class ProductService(IProductRepository productRepository): IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public Guid Id { get; } = Guid.NewGuid();

        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToResponse).ToList() is List<ProductResponseDto> response
                ? response
                : throw new Exception("Failed to map products to response.");
        }

        private ProductResponseDto MapToResponse(Product product)
        {
            return new ProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                product.StockQuantity
            );
        }
    }
}

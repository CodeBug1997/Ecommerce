using Ecommerce.Service.Dtos.ProductDtos;

namespace Ecommerce.Service.Services
{
    public interface IProductService
    {
        Guid Id { get; }

         Task<List<ProductResponseDto>> GetAllProductsAsync();
    }
}

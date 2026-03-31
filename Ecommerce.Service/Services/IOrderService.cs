using Ecommerce.Repository.Entities;
using Ecommerce.Service.Dtos.OrderDtos;

namespace Ecommerce.Service.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request);
        Task<Order?> CancelOrderAsync(long id);
        Task<OrderResponseDto?> GetOrderAsync(long id);
    }
}
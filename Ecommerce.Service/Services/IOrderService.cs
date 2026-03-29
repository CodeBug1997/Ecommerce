using Ecommerce.Repository.Entities;
using Ecommerce.Service.Dtos;

namespace Ecommerce.Service.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
    }
}
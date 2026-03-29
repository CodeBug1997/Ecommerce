using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetTop5NewestOrdersAsync();
        Task<long> CreateAsync(Order order, IDbTransaction transaction);
        Task CreateItemsAsync(IEnumerable<OrderItem> items, IDbTransaction transaction);
        Task<Order?> GetWithItemsAsync(long id);
    }
}

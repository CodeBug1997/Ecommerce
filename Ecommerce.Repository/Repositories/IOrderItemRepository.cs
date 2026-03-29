using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task CreateItemsAsync(IEnumerable<OrderItem> items, IDbTransaction transaction);
    }
}

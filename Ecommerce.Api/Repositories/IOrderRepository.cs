using Ecommerce.Api.Entities;

namespace Ecommerce.Api.Repositories
{
    public  interface  IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetTop5NewestOrdersAsync();
    }
}

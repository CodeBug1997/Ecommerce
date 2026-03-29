using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<long> ids);
        Task<int> DecreaseStockAsync(long productId, int quantity, IDbTransaction transaction);
    }
}

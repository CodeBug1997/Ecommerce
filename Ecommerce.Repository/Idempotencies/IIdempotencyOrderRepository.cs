using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Idempotencies
{
    public interface IIdempotencyOrderRepository : IIdempotencyRepository<IdempotencyOrder>
    {
        Task<int> SaveAsync(string key, long id);
        Task<int> SaveAsync(string key, long id, IDbTransaction transaction);
    }
}
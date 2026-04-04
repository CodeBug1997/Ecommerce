using System.Data;

namespace Ecommerce.Repository.Common
{
    public interface IIdempotencyRepository<T>
    {
        Task<T?> GetAsync(string key);
    }
}

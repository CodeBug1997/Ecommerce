using System.Data;

namespace Ecommerce.Repository.Common
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(long id);
        Task<T?> GetByIdAsync(long id, IDbTransaction transaction);
        Task<IEnumerable<T>> GetByIdsAsync(List<long> ids);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
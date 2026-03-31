namespace Ecommerce.Repository.Common
{
    public interface IIdempotencyRepository<T>
    {
        Task<T?> GetAsync(string key);
        Task<int> SaveAsync(string key, long id);
    }
}

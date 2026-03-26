namespace Ecommerce.Api.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}

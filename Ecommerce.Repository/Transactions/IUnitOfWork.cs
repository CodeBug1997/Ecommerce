using System.Data;

namespace Ecommerce.Repository.Transactions
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}

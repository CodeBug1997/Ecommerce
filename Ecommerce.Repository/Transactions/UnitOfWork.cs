using System.Data;
using System.Data.Common;

namespace Ecommerce.Repository.Transactions
{
    public class UnitOfWork(IDbConnection connection) : IUnitOfWork
    {
        private readonly IDbConnection _connection = connection;
        public IDbConnection Connection => _connection;

        private IDbTransaction? _transaction;
        public IDbTransaction Transaction
        {
            get => _transaction ?? throw new InvalidOperationException("Transaction has not been started.");
            private set => _transaction = value;
        }

        public async Task BeginAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already started.");

            if (_connection.State != ConnectionState.Open)
            {
                if (_connection is DbConnection dbConnection)
                {
                    await dbConnection.OpenAsync();
                }
                else
                {
                    _connection.Open();
                }
            }

            _transaction = _connection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            Transaction.Commit();
            Transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            Transaction.Rollback();
            Transaction.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }
    }
}

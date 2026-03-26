using Dapper;
using System.Data;

namespace Ecommerce.Api.Repositories
{
    public class DapperRepository<T> : IRepository<T>
    {
        protected readonly IDbConnection _connection;
        public DapperRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

            return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName}";

            return await _connection.QueryAsync<T>(sql);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var tableName = GetTableName();
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        protected virtual string GetTableName()
        {
            return typeof(T).Name;
        }
    }
}

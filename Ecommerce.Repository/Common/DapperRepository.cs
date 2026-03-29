using Dapper;
using Ecommerce.Base.Attributes;
using System.Data;
using System.Reflection;

namespace Ecommerce.Repository.Common
{
    public class DapperRepository<T>(IDbConnection connection) : IRepository<T>
    {
        protected readonly IDbConnection _connection = connection;

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<T?> GetByIdAsync(long id, IDbTransaction transaction)
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
            return await transaction.Connection!.QueryFirstOrDefaultAsync<T>(sql, new { Id = id }, transaction);
        }

        public virtual async Task<IEnumerable<T>> GetByIdsAsync(List<long> ids)
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE Id IN @Ids";
            return await _connection.QueryAsync<T>(sql, new { Ids = ids});
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
            var attr = typeof(T).GetCustomAttribute<TableAttribute>();

            return attr == null ? throw new Exception($"Table attribute not found for {typeof(T).Name}") : attr.Name;
        }
    }
}

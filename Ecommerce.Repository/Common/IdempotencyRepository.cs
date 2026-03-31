using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Ecommerce.Repository.Common
{
    public class IdempotencyRepository<T>(IDbConnection connection) : IIdempotencyRepository<T>
    {
        protected readonly IDbConnection _connection = connection;
        public async Task<T?> GetAsync(string key)
        {
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE idempotency_key = @Key";
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { Key = key });
        }

        public async Task<int> SaveAsync(string key, long id)
        {
            var tableName = GetTableName();
            var sql = $"INSERT INTO {tableName} VALUES (@Key,@IdempotencyId,SYSUTCDATETIME())";
            return await _connection.ExecuteAsync(sql, new { Key = key, IdempotencyId = id });
        }

        protected virtual string GetTableName()
        {
            var attr = typeof(T).GetCustomAttribute<TableAttribute>();

            return attr == null ? throw new Exception($"Table attribute not found for {typeof(T).Name}") : attr.Name;
        }
    }
}

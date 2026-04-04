using Dapper;
using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Idempotencies
{
    public class IdempotencyOrderRepository(IDbConnection connection) : IdempotencyRepository<IdempotencyOrder>(connection), IIdempotencyOrderRepository
    {
        public async Task<int> SaveAsync(string key, long id)
        {
            var sql = $"INSERT INTO idempotency_orders (idempotency_key, order_id, created_at) VALUES (@Key, @OrderId, SYSUTCDATETIME())";
            return await _connection.ExecuteAsync(sql, new { Key = key, OrderId = id });
        }
        public async Task<int> SaveAsync(string key, long id, IDbTransaction transaction)
        {
            var sql = $"INSERT INTO idempotency_orders (idempotency_key, order_id, created_at) VALUES (@Key, @OrderId, SYSUTCDATETIME())";
            return await _connection.ExecuteAsync(sql, new { Key = key, OrderId = id }, transaction);
        }
    }
}
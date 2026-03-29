using Dapper;
using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public class OrderRepository(IDbConnection connection) : DapperRepository<Order>(connection), IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetTop5NewestOrdersAsync()
        {
            const string sql = @"SELECT TOP 5 * FROM orders ORDER BY created_at DESC";
            return await _connection.QueryAsync<Order>(sql);
        }

        public async Task<long> CreateAsync(Order order, IDbTransaction transaction)
        {
            const string sql = @"
            INSERT INTO orders (created_at, total_amount, status)
            OUTPUT INSERTED.Id
            VALUES (@CreatedAt, @TotalAmount, @Status);";

            var orderId = await transaction.Connection!.ExecuteScalarAsync<long>(
                sql,
                new
                {
                    order.CreatedAt,
                    order.TotalAmount,
                    order.Status
                },
                transaction);

            return orderId;
        }

        public async Task CreateItemsAsync(IEnumerable<OrderItem> items, IDbTransaction transaction)
        {
            const string sql = @"
            INSERT INTO order_items (order_id, product_id, quantity, unit_price, line_total)
            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @LineTotal);";

            await transaction.Connection!.ExecuteAsync(sql, items, transaction);
        }

        public Task<Order?> GetWithItemsAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}

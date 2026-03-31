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

        public async Task<Order?> GetWithItemsAsync(long id)
        {
            var orderSql = @"
            SELECT id, total_amount, status, created_at
            FROM orders
            WHERE id = @OrderId";

            var itemSql = @"
            SELECT id, order_id, product_id, quantity, unit_price
            FROM order_items
            WHERE order_id = @OrderId";

            var order = await _connection.QueryFirstOrDefaultAsync<Order>(orderSql, new { OrderId = id });

            if (order == null)
                return null;

            var items = await _connection.QueryAsync<OrderItem>(itemSql, new { OrderId = id });
            order.Items = [.. items];

            return order;
        }

        public async Task<Order?> GetWithItemsAsync(long id, IDbTransaction transaction)
        {
            var orderSql = @"
            SELECT id, total_amount, status, created_at
            FROM orders
            WHERE id = @OrderId";

            var itemSql = @"
            SELECT id, order_id, product_id, quantity, unit_price
            FROM order_items
            WHERE order_id = @OrderId";

            var order = await transaction.Connection!.QueryFirstOrDefaultAsync<Order>(orderSql, new { OrderId = id }, transaction);

            if (order == null)
                return null;

            var items = await transaction.Connection!.QueryAsync<OrderItem>(itemSql, new { OrderId = id }, transaction);
            order.Items = [.. items];

            return order;
        }

        public async Task<int> UpdateAsync(Order order, IDbTransaction transaction)
        {
            const string sql = @"
            UPDATE orders
            SET 
                total_amount = @TotalAmount,
                status = @Status
            WHERE id = @Id;";
            return await transaction.Connection!.ExecuteAsync(sql, order, transaction);
        }
    }
}

using Dapper;
using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public class OrderItemRepository(IDbConnection connection) : DapperRepository<OrderItem>(connection), IOrderItemRepository
    {
        public async Task CreateItemsAsync(IEnumerable<OrderItem> items, IDbTransaction transaction)
        {
            const string sql = @"
            INSERT INTO order_items (order_id, product_id, quantity, unit_price, line_total)
            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @LineTotal);";

            await transaction.Connection!.ExecuteAsync(sql, items, transaction);
        }
    }
}

using Dapper;
using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using System.Data;

namespace Ecommerce.Repository.Repositories
{
    public class ProductRepository(IDbConnection connection) : DapperRepository<Product>(connection), IProductRepository
    {
        public async Task<int> DecreaseStockAsync(long productId, int quantity, IDbTransaction transaction)
        {
            var sql = @"UPDATE products 
            SET stock_quantity = stock_quantity - @Quantity,
                updated_at = SYSUTCDATETIME()
                WHERE id = @ProductId AND stock_quantity >= @Quantity;";
            return await transaction.Connection!.ExecuteAsync(sql, new { Quantity = quantity, ProductId = productId }, transaction);
        }

        public async Task<int> IncreaseStockAsync(long productId, int quantity, IDbTransaction transaction)
        {
            var sql = @"UPDATE products 
            SET stock_quantity = stock_quantity + @Quantity,
                updated_at = SYSUTCDATETIME()
                WHERE id = @ProductId;";
            return await transaction.Connection!.ExecuteAsync(sql, new { Quantity = quantity, ProductId = productId }, transaction);
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<long> ids)
        {
            const string sql = @"SELECT * FROM products WHERE id IN @Ids";
            return await _connection.QueryAsync<Product>(sql, new { Ids = ids });
        }
    }
}

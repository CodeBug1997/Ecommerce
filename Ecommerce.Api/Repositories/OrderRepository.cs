using Dapper;
using Ecommerce.Api.Entities;
using System.Data;

namespace Ecommerce.Api.Repositories
{
    public class OrderRepository : DapperRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            var sql = $"SELECT * FROM Orders WHERE user_id = @UserId";
            return await _connection.QueryAsync<Order>(sql, new { UserUd = userId });
        }

        public async Task<IEnumerable<Order>> GetTop5NewestOrdersAsync()
        {
            var sql = $"SELECT * FROM Orders ORDER BY create_date DESC LIMIT 5";
            return await _connection.QueryAsync<Order>(sql);
        }
    }
}

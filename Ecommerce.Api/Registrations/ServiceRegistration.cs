using Ecommerce.Repository.Common;
using Ecommerce.Repository.Repositories;
using Ecommerce.Repository.Transactions;
using Ecommerce.Service.Idempotency;
using Ecommerce.Service.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.Api.Registrations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IDbConnection>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            });
            services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
            services.AddScoped(typeof(IIdempotencyRepository<>), typeof(IdempotencyRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.Decorate<IOrderService, IdempotencyOrderServiceDecorator>();

            return services;
        }
    }
}

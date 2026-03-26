using Ecommerce.Api.Repositories;
using Ecommerce.Api.Services.Category;
using Ecommerce.Api.Services.Product;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ecommerce.Api.Registrations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IDbConnection>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new MySqlConnection(connectionString);
            });
            services.AddTransient<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            
            services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}

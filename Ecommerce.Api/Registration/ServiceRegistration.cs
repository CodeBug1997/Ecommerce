using Ecommerce.Api.Services.Category;
using Ecommerce.Api.Services.Product;

namespace Ecommerce.Api.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<CategoryService>();
            services.AddTransient<ProductService>();
            return services;
        }
    }
}

using Ecommerce.Api.Services.Category;
using Ecommerce.Api.Services.Product;

namespace Ecommerce.Api.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ICategoryService,CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            return services;
        }
    }
}

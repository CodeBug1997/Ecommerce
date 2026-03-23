using Ecommerce.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Ecommerce.Api.Registration
{
    public static class MiddlewareRegistration
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            return app;
        }
    }
}

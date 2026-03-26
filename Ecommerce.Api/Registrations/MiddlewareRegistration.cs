using Ecommerce.Api.Middlewares;

namespace Ecommerce.Api.Registrations
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

using Ecommerce.Base.Exeptions;

namespace Ecommerce.Api.Middlewares
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException)
            {
                context.Response.StatusCode = 400;
            }
            catch (NotFoundException)
            {
                context.Response.StatusCode = 404;
            }
            catch (ConflictException)
            {
                context.Response.StatusCode = 409;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
            }
        }
    }
}

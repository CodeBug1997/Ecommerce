namespace Ecommerce.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.UtcNow;

            Console.WriteLine($"[Request] {context.Request.Path}");

            await _next(context);

            var duration = DateTime.UtcNow - start;

            Console.WriteLine($"[Response] {context.Request.Path} took {duration.TotalMilliseconds} ms");
        }
    }
}

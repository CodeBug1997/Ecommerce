namespace Ecommerce.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.UtcNow;
            var requestId = Guid.NewGuid();

            _logger.LogInformation("[{RequestId}] {Method} {Path}",
                requestId, context.Request.Method, context.Request.Path);
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "[{RequestId}] Exception for {Method} {Path}",
                   requestId,
                   context.Request.Method,
                   context.Request.Path);
                throw;
            }
            
            var duration = DateTime.UtcNow - start;
            _logger.LogInformation(
               "[{RequestId}] [Response] {Method} {Path} {StatusCode} took {Duration} ms",
               requestId,
               context.Request.Method,
               context.Request.Path,
               context.Response.StatusCode,
               duration.TotalMilliseconds);
        }
    }
}

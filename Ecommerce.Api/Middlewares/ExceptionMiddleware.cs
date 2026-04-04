using Ecommerce.Base.Exeptions;
using System.Text.Json;

namespace Ecommerce.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex, "[{TraceId}] Exception occurred", traceId);
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("[{TraceId}] Response has already started", traceId);
                    throw;
                }
                var (statusCode, body) = MapException(ex, traceId);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(body));
            }
        }

        private static (int StatusCode, ErrorResponse Body) MapException(Exception ex, string traceId)
        {
            return ex switch
            {
                ValidationException e => (400, new ErrorResponse
                {
                    Code = "VALIDATION_ERROR",
                    Message = e.Message,
                    TraceId = traceId
                }),

                BadRequestException e => (400, new ErrorResponse
                {
                    Code = "BAD_REQUEST",
                    Message = e.Message,
                    TraceId = traceId
                }),

                NotFoundException e => (404, new ErrorResponse
                {
                    Code = "NOT_FOUND",
                    Message = e.Message,
                    TraceId = traceId
                }),

                OutOfStockException e => (409, new ErrorResponse
                {
                    Code = "ORDER_OUT_OF_STOCK",
                    Message = e.Message,
                    TraceId = traceId
                }),

                ConflictException e => (409, new ErrorResponse
                {
                    Code = "CONFLICT",
                    Message = e.Message,
                    TraceId = traceId
                }),

                _ => (500, new ErrorResponse
                {
                    Code = "INTERNAL_ERROR",
                    Message = "Something went wrong",
                    TraceId = traceId
                })
            };
        }

        public sealed class ErrorResponse
        {
            public string Code { get; init; } = default!;
            public string Message { get; init; } = default!;
            public string TraceId { get; init; } = default!;
            public object? Details { get; init; }
        }

    }
}

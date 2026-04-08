using Dapper;
using Ecommerce.Api.Registrations;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Open telemetry configuration
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "Ecommerce.Api",
            serviceVersion: "1.0.0")
        .AddAttributes(
        [
            new KeyValuePair<string, object>("deployment.environment", builder.Environment.EnvironmentName)
        ]))
    .WithTracing(tracing => tracing
        .AddSource("Ecommerce.Service")
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddHttpClientInstrumentation()
        .AddConsoleExporter());

builder.Services.AddCustomServices(builder.Configuration);
DefaultTypeMap.MatchNamesWithUnderscores = true;
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}
app.UseCustomMiddlewares();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
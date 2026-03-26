using Dapper;
using Ecommerce.Api.Registrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
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
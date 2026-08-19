using SafetyCompliance.API;
using SafetyCompliance.API.Middlewares;
using SafetyCompliance.Application;
using SafetyCompliance.Infrastructure;
using SafetyCompliance.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration)
                .AddApplicationServices()
                .AddInfrastructureService()
                .AddApiServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "SafetyCompliance");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ApiResponseMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

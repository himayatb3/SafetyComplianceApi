using Microsoft.AspNetCore.Mvc;
using SafetyCompliance.API.Filters;

namespace SafetyCompliance.API
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddProblemDetails();

            services.AddControllers(options =>
            {
                options.Filters.Add<ApiResponseResultFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Detail = "One or more validation errors occurred.",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            return services;
        }
    }
}

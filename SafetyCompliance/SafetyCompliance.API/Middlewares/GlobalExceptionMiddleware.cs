using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SafetyCompliance.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Unhandled exception occurred while processing the request.");
                await WriteProblemDetailsAsync(context, exception);
            }
        }

        private static async Task WriteProblemDetailsAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw exception;
            }

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = "An unexpected error occurred while processing the request.",
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
        }
    }
}

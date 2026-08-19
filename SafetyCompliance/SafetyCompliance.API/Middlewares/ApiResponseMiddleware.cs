using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SafetyCompliance.API.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);

            if (context.Response.HasStarted || context.Response.StatusCode < 400)
            {
                return;
            }

            if (context.Response.ContentLength.HasValue || context.Response.Body.CanSeek && context.Response.Body.Length > 0)
            {
                return;
            }

            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = GetTitle(context.Response.StatusCode),
                Detail = "The request could not be completed.",
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
        }

        private static string GetTitle(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Resource not found",
                StatusCodes.Status409Conflict => "Conflict",
                _ => "Request failed"
            };
        }
    }
}

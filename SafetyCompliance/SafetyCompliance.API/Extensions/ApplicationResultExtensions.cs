using Microsoft.AspNetCore.Mvc;
using SafetyCompliance.Application.Common.Results;

namespace SafetyCompliance.API.Extensions
{
    public static class ApplicationResultExtensions
    {
        public static int ToStatusCode(this IApplicationResult result)
        {
            if (result.IsSuccess)
            {
                return StatusCodes.Status200OK;
            }

            return result.ErrorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        public static ProblemDetails ToProblemDetails(this IApplicationResult result, HttpContext httpContext)
        {
            var statusCode = result.ToStatusCode();
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(result.ErrorType),
                Detail = result.Message,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["errors"] = result.Errors.Select(error => new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            });

            return problemDetails;
        }

        private static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "Validation failed",
                ErrorType.NotFound => "Resource not found",
                ErrorType.Conflict => "Conflict",
                ErrorType.Unauthorized => "Unauthorized",
                ErrorType.Forbidden => "Forbidden",
                _ => "Request failed"
            };
        }
    }
}

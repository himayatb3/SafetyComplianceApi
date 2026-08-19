using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SafetyCompliance.API.Extensions;
using SafetyCompliance.Application.Common.Responses;
using SafetyCompliance.Application.Common.Results;

namespace SafetyCompliance.API.Filters
{
    public class ApiResponseResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult { Value: IApplicationResult result } objectResult)
            {
                context.Result = result.IsSuccess
                    ? CreateSuccessResult(result, objectResult, context.HttpContext)
                    : CreateProblemResult(result, context.HttpContext);
            }

            await next();
        }

        private static ObjectResult CreateSuccessResult(
            IApplicationResult result,
            ObjectResult objectResult,
            HttpContext httpContext)
        {
            var response = ApiResponse<object>.Ok(
                result.GetValue(),
                result.Message,
                httpContext.TraceIdentifier);

            return new ObjectResult(response)
            {
                StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK
            };
        }

        private static ObjectResult CreateProblemResult(IApplicationResult result, HttpContext httpContext)
        {
            var problemDetails = result.ToProblemDetails(httpContext);

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}

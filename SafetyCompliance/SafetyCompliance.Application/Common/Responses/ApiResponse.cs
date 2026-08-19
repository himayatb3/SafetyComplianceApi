namespace SafetyCompliance.Application.Common.Responses
{
    public sealed class ApiResponse<T>
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;

        public T? Data { get; init; }

        public string? TraceId { get; init; }

        public static ApiResponse<T> Ok(T? data, string message, string? traceId)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = traceId
            };
        }
    }
}

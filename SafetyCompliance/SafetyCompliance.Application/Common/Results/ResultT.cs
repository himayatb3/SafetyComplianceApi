namespace SafetyCompliance.Application.Common.Results
{
    public sealed class Result<T> : IApplicationResult
    {
        private Result(bool isSuccess, T? value, string message, IReadOnlyCollection<ApplicationError> errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Message = message;
            Errors = errors;
            ErrorType = errors.FirstOrDefault()?.Type ?? ErrorType.Failure;
        }

        public bool IsSuccess { get; }

        public T? Value { get; }

        public string Message { get; }

        public ErrorType ErrorType { get; }

        public IReadOnlyCollection<ApplicationError> Errors { get; }

        public static Result<T> Success(T value, string message = "Request completed successfully.")
        {
            return new Result<T>(true, value, message, Array.Empty<ApplicationError>());
        }

        public static Result<T> Failure(string message, ErrorType errorType = ErrorType.Failure)
        {
            return new Result<T>(false, default, message, new[] { new ApplicationError(errorType.ToString(), message, errorType) });
        }

        public static Result<T> Failure(ApplicationError error)
        {
            return new Result<T>(false, default, error.Message, new[] { error });
        }

        public static Result<T> Failure(IEnumerable<ApplicationError> errors, string message = "Request failed.")
        {
            var errorList = errors.ToArray();
            return new Result<T>(false, default, message, errorList);
        }

        public object? GetValue()
        {
            return Value;
        }
    }
}

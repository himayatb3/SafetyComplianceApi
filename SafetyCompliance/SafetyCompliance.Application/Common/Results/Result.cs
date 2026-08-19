namespace SafetyCompliance.Application.Common.Results
{
    public class Result : IApplicationResult
    {
        protected Result(bool isSuccess, string message, IReadOnlyCollection<ApplicationError> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
            ErrorType = errors.FirstOrDefault()?.Type ?? ErrorType.Failure;
        }

        public bool IsSuccess { get; }

        public string Message { get; }

        public ErrorType ErrorType { get; }

        public IReadOnlyCollection<ApplicationError> Errors { get; }

        public static Result Success(string message = "Request completed successfully.")
        {
            return new Result(true, message, Array.Empty<ApplicationError>());
        }

        public static Result Failure(string message, ErrorType errorType = ErrorType.Failure)
        {
            return new Result(false, message, new[] { new ApplicationError(errorType.ToString(), message, errorType) });
        }

        public static Result Failure(ApplicationError error)
        {
            return new Result(false, error.Message, new[] { error });
        }

        public static Result Failure(IEnumerable<ApplicationError> errors, string message = "Request failed.")
        {
            var errorList = errors.ToArray();
            return new Result(false, message, errorList);
        }

        public object? GetValue()
        {
            return null;
        }
    }
}

namespace SafetyCompliance.Application.Common.Results
{
    public sealed class ApplicationError
    {
        public ApplicationError(string code, string message, ErrorType type = ErrorType.Failure)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public string Code { get; }

        public string Message { get; }

        public ErrorType Type { get; }
    }
}

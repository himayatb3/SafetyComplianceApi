namespace SafetyCompliance.Application.Common.Results
{
    public interface IApplicationResult
    {
        bool IsSuccess { get; }

        string Message { get; }

        ErrorType ErrorType { get; }

        IReadOnlyCollection<ApplicationError> Errors { get; }

        object? GetValue();
    }
}

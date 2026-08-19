using SafetyCompliance.Application.DTOs.Account;
using SafetyCompliance.Application.Common.Results;

namespace SafetyCompliance.Application.Abstraction.IService
{
    public interface IAccountService
    {
        Task<Result<UserDto>> CreateEmployeeAccountAsync(CreateEmployeeAccountRequest request);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }
}

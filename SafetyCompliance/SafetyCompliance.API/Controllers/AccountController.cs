using Microsoft.AspNetCore.Mvc;
using SafetyCompliance.Application.Abstraction.IService;
using SafetyCompliance.Application.Common.Results;
using SafetyCompliance.Application.DTOs.Account;

namespace SafetyCompliance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("employees")]
        public Task<Result<UserDto>> CreateEmployeeAccount(CreateEmployeeAccountRequest request)
        {
            return accountService.CreateEmployeeAccountAsync(request);
        }

        [HttpPost("login")]
        public Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            return accountService.LoginAsync(request);
        }
    }
}

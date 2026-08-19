using SafetyCompliance.Application.Abstraction.IJWT;
using SafetyCompliance.Application.Abstraction.IRepository;
using SafetyCompliance.Application.Abstraction.ISecurity;
using SafetyCompliance.Application.Abstraction.IService;
using SafetyCompliance.Application.Common.Results;
using SafetyCompliance.Application.DTOs.Account;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJWTService jwtService;

        public AccountService(
            IAccountRepository accountRepository,
            IPasswordHasher passwordHasher,
            IJWTService jwtService)
        {
            this.accountRepository = accountRepository;
            this.passwordHasher = passwordHasher;
            this.jwtService = jwtService;
        }

        public async Task<Result<UserDto>> CreateEmployeeAccountAsync(CreateEmployeeAccountRequest request)
        {
            var validationErrors = ValidateCreateEmployeeRequest(request).ToArray();
            if (validationErrors.Length > 0)
            {
                return Result<UserDto>.Failure(validationErrors, "User account validation failed.");
            }

            if (await accountRepository.IsUserNameExistsAsync(request.UserName.Trim()))
            {
                return Result<UserDto>.Failure(new ApplicationError(
                    "User.UserNameAlreadyExists",
                    "Username already exists.",
                    ErrorType.Conflict));
            }

            if (await accountRepository.IsEmailExistsAsync(request.Email.Trim()))
            {
                return Result<UserDto>.Failure(new ApplicationError(
                    "User.EmailAlreadyExists",
                    "Email already exists.",
                    ErrorType.Conflict));
            }

            var salt = passwordHasher.GenerateSalt();
            var user = new User
            {
                UserName = request.UserName.Trim(),
                Email = request.Email.Trim(),
                Salt = salt,
                PasswordHash = passwordHasher.HashPassword(request.TemporaryPassword, salt),
                IsActive = true,
                IsDeleted = false,
                MustChangePassword = true,
                CreatedBy = request.CreatedBy,
                Profile = new UserProfile
                {
                    CompanyId = request.CompanyId,
                    SiteId = request.SiteId,
                    FirstName = request.FirstName.Trim(),
                    MiddleName = request.MiddleName.Trim(),
                    LastName = request.LastName.Trim(),
                    EmployeeCode = request.EmployeeCode.Trim(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    Department = request.Department.Trim(),
                    Designation = request.Designation.Trim(),
                    CreatedBy = request.CreatedBy
                }
            };

            foreach (var roleId in request.RoleIds.Distinct())
            {
                user.UserRoles.Add(new UserRole
                {
                    RoleId = roleId,
                    CreatedBy = request.CreatedBy
                });
            }

            await accountRepository.AddAsync(user);

            return Result<UserDto>.Success(ToUserDto(user), "Employee account created successfully.");
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserNameOrEmail) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result<LoginResponse>.Failure(new ApplicationError(
                    "Login.InvalidCredentials",
                    "Username/email and password are required.",
                    ErrorType.Validation));
            }

            var user = await accountRepository.GetByUserNameOrEmailAsync(request.UserNameOrEmail.Trim());
            if (user is null || !passwordHasher.VerifyPassword(request.Password, user.Salt, user.PasswordHash))
            {
                return Result<LoginResponse>.Failure(new ApplicationError(
                    "Login.InvalidCredentials",
                    "Invalid username/email or password.",
                    ErrorType.Unauthorized));
            }

            if (user.IsDeleted || !user.IsActive)
            {
                return Result<LoginResponse>.Failure(new ApplicationError(
                    "Login.AccountInactive",
                    "User account is inactive.",
                    ErrorType.Forbidden));
            }

            if (user.IsBlocked)
            {
                return Result<LoginResponse>.Failure(new ApplicationError(
                    "Login.AccountBlocked",
                    "User account is blocked.",
                    ErrorType.Forbidden));
            }

            user.LoginCount++;
            user.LastLoggedIn = DateTime.UtcNow;
            user.WrongLoginCount = 0;
            await accountRepository.UpdateAsync(user);

            return Result<LoginResponse>.Success(new LoginResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = jwtService.GenerateTokenAsync(user),
                MustChangePassword = user.MustChangePassword
            }, "Login successful.");
        }

        private static IEnumerable<ApplicationError> ValidateCreateEmployeeRequest(CreateEmployeeAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                yield return new ApplicationError("User.UserNameRequired", "Username is required.", ErrorType.Validation);
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                yield return new ApplicationError("User.EmailRequired", "Email is required.", ErrorType.Validation);
            }

            if (string.IsNullOrWhiteSpace(request.TemporaryPassword) || request.TemporaryPassword.Length < 8)
            {
                yield return new ApplicationError("User.PasswordInvalid", "Temporary password must be at least 8 characters.", ErrorType.Validation);
            }

            if (request.CompanyId == Guid.Empty)
            {
                yield return new ApplicationError("User.CompanyRequired", "Company is required.", ErrorType.Validation);
            }

            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                yield return new ApplicationError("User.FirstNameRequired", "First name is required.", ErrorType.Validation);
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                yield return new ApplicationError("User.LastNameRequired", "Last name is required.", ErrorType.Validation);
            }

            if (request.RoleIds.Count == 0)
            {
                yield return new ApplicationError("User.RoleRequired", "At least one role is required.", ErrorType.Validation);
            }
        }

        private static UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = string.Join(" ", new[]
                {
                    user.Profile?.FirstName,
                    user.Profile?.MiddleName,
                    user.Profile?.LastName
                }.Where(name => !string.IsNullOrWhiteSpace(name))),
                CompanyId = user.Profile?.CompanyId ?? Guid.Empty,
                SiteId = user.Profile?.SiteId,
                IsActive = user.IsActive,
                MustChangePassword = user.MustChangePassword
            };
        }
    }
}

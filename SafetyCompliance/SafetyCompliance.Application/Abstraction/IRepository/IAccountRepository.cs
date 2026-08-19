using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Application.Abstraction.IRepository
{
    public interface IAccountRepository : IBaseRepository<User>
    {
        Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail);

        Task<bool> IsUserNameExistsAsync(string userName);

        Task<bool> IsEmailExistsAsync(string email);
    }
}

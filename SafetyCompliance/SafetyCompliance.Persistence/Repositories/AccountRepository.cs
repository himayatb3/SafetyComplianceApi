using Microsoft.EntityFrameworkCore;
using SafetyCompliance.Application.Abstraction.IRepository;
using SafetyCompliance.Domain.Entity;
using SafetyCompliance.Persistence.Data;
using SafetyCompliance.Persistence.Repositories.BaseRepository;

namespace SafetyCompliance.Persistence.Repositories
{
    public class AccountRepository : BaseRepository<User>, IAccountRepository
    {
        private readonly SafetyComplianceDbContext context;

        public AccountRepository(SafetyComplianceDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail)
        {
            return context.Users
                .Include(user => user.Profile)
                .Include(user => user.UserRoles)
                .FirstOrDefaultAsync(user =>
                    user.UserName == userNameOrEmail ||
                    user.Email == userNameOrEmail);
        }

        public Task<bool> IsUserNameExistsAsync(string userName)
        {
            return context.Users.AnyAsync(user => user.UserName == userName);
        }

        public Task<bool> IsEmailExistsAsync(string email)
        {
            return context.Users.AnyAsync(user => user.Email == email);
        }
    }
}

using SafetyCompliance.Application.Abstraction.IRepository;
using SafetyCompliance.Domain.Entity;
using SafetyCompliance.Persistence.Data;
using SafetyCompliance.Persistence.Repositories.BaseRepository;

namespace SafetyCompliance.Persistence.Repositories
{
    public class AccountRepository:BaseRepository<User>, IAccountRepository
    {
        private readonly SafetyComplianceDbContext context;

        public AccountRepository(SafetyComplianceDbContext context):base(context)
        {
            this.context = context;
        }
    }
}

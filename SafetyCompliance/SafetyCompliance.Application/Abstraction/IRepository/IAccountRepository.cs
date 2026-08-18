using SafetyCompliance.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyCompliance.Application.Abstraction.IRepository
{
    public interface IAccountRepository : IBaseRepository<User>
    {
    }
}

using Microsoft.EntityFrameworkCore;
using SafetyCompliance.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyCompliance.Persistence.Data
{
    public class SafetyComplianceDbContext : DbContext
    {
        private readonly DbContextOptions<SafetyComplianceDbContext> context;

        public SafetyComplianceDbContext(DbContextOptions<SafetyComplianceDbContext> context) : base(context)
        {
            this.context = context;
        }

        public DbSet<User> Users {get;set;}
    }
}

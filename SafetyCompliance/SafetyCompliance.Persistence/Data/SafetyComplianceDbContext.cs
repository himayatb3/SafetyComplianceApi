using Microsoft.EntityFrameworkCore;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data
{
    public class SafetyComplianceDbContext : DbContext
    {
        public SafetyComplianceDbContext(DbContextOptions<SafetyComplianceDbContext> context) : base(context)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; }

        public DbSet<UserMenuAccessOverride> UserMenuAccessOverrides { get; set; }

        public DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; }

        public DbSet<RoleWorkflowAccess> RoleWorkflowAccesses { get; set; }

        public DbSet<UserWorkflowAccessOverride> UserWorkflowAccessOverrides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SafetyComplianceDbContext).Assembly);
        }
    }
}

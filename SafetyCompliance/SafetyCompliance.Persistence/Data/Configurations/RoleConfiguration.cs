using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasOne(role => role.Company)
                .WithMany(company => company.Roles)
                .HasForeignKey(role => role.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(role => new { role.CompanyId, role.Code })
                .IsUnique();
        }
    }
}

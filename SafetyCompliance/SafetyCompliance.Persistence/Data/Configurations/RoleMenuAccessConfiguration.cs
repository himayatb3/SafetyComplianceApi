using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class RoleMenuAccessConfiguration : IEntityTypeConfiguration<RoleMenuAccess>
    {
        public void Configure(EntityTypeBuilder<RoleMenuAccess> builder)
        {
            builder.HasOne(access => access.Role)
                .WithMany(role => role.MenuAccesses)
                .HasForeignKey(access => access.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(access => access.MenuItem)
                .WithMany(menuItem => menuItem.RoleAccesses)
                .HasForeignKey(access => access.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(access => new { access.RoleId, access.MenuItemId })
                .IsUnique();
        }
    }
}

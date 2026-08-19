using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class UserMenuAccessOverrideConfiguration : IEntityTypeConfiguration<UserMenuAccessOverride>
    {
        public void Configure(EntityTypeBuilder<UserMenuAccessOverride> builder)
        {
            builder.HasOne(access => access.User)
                .WithMany(user => user.MenuAccessOverrides)
                .HasForeignKey(access => access.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(access => access.MenuItem)
                .WithMany(menuItem => menuItem.UserAccessOverrides)
                .HasForeignKey(access => access.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(access => new { access.UserId, access.MenuItemId })
                .IsUnique();
        }
    }
}

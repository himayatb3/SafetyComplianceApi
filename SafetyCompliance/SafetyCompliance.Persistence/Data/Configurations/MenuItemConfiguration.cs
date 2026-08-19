using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasOne(menuItem => menuItem.Module)
                .WithMany(module => module.MenuItems)
                .HasForeignKey(menuItem => menuItem.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(menuItem => menuItem.ParentMenuItem)
                .WithMany(menuItem => menuItem.ChildMenuItems)
                .HasForeignKey(menuItem => menuItem.ParentMenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(menuItem => new { menuItem.ModuleId, menuItem.Code })
                .IsUnique();
        }
    }
}

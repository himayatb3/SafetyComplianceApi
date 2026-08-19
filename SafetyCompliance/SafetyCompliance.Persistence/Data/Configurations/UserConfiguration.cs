using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.UserName)
                .IsUnique();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.HasOne(user => user.Profile)
                .WithOne(profile => profile.User)
                .HasForeignKey<UserProfile>(profile => profile.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

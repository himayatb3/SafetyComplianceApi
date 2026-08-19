using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(profile => profile.Company)
                .WithMany(company => company.UserProfiles)
                .HasForeignKey(profile => profile.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(profile => profile.Site)
                .WithMany(site => site.UserProfiles)
                .HasForeignKey(profile => profile.SiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(profile => profile.UserId)
                .IsUnique();
        }
    }
}

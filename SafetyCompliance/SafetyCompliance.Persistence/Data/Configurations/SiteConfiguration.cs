using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.HasOne(site => site.Company)
                .WithMany(company => company.Sites)
                .HasForeignKey(site => site.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(site => new { site.CompanyId, site.Code })
                .IsUnique();
        }
    }
}

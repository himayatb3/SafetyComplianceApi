using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasIndex(module => module.Code)
                .IsUnique();
        }
    }
}

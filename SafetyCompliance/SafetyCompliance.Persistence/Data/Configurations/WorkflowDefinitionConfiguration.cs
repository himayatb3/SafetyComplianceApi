using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
    {
        public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
        {
            builder.HasOne(workflow => workflow.Module)
                .WithMany(module => module.Workflows)
                .HasForeignKey(workflow => workflow.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(workflow => workflow.Company)
                .WithMany()
                .HasForeignKey(workflow => workflow.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(workflow => new { workflow.ModuleId, workflow.CompanyId, workflow.Code })
                .IsUnique();
        }
    }
}

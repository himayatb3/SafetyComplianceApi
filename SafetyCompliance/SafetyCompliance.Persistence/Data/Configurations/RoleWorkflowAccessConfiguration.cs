using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class RoleWorkflowAccessConfiguration : IEntityTypeConfiguration<RoleWorkflowAccess>
    {
        public void Configure(EntityTypeBuilder<RoleWorkflowAccess> builder)
        {
            builder.HasOne(access => access.Role)
                .WithMany(role => role.WorkflowAccesses)
                .HasForeignKey(access => access.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(access => access.WorkflowDefinition)
                .WithMany(workflow => workflow.RoleAccesses)
                .HasForeignKey(access => access.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(access => new { access.RoleId, access.WorkflowDefinitionId })
                .IsUnique();
        }
    }
}

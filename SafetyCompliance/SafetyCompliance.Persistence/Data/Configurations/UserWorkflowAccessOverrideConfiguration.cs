using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Persistence.Data.Configurations
{
    public class UserWorkflowAccessOverrideConfiguration : IEntityTypeConfiguration<UserWorkflowAccessOverride>
    {
        public void Configure(EntityTypeBuilder<UserWorkflowAccessOverride> builder)
        {
            builder.HasOne(access => access.User)
                .WithMany(user => user.WorkflowAccessOverrides)
                .HasForeignKey(access => access.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(access => access.WorkflowDefinition)
                .WithMany(workflow => workflow.UserAccessOverrides)
                .HasForeignKey(access => access.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(access => new { access.UserId, access.WorkflowDefinitionId })
                .IsUnique();
        }
    }
}

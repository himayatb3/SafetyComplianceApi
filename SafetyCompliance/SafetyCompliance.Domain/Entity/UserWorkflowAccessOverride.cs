namespace SafetyCompliance.Domain.Entity
{
    public class UserWorkflowAccessOverride : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid WorkflowDefinitionId { get; set; }

        public bool IsAllowed { get; set; }

        public bool CanStart { get; set; }

        public bool CanView { get; set; }

        public bool CanAction { get; set; }

        public bool CanAdminister { get; set; }

        public User? User { get; set; }

        public WorkflowDefinition? WorkflowDefinition { get; set; }
    }
}

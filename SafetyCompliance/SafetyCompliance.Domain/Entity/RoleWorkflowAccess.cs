namespace SafetyCompliance.Domain.Entity
{
    public class RoleWorkflowAccess : BaseEntity
    {
        public Guid RoleId { get; set; }

        public Guid WorkflowDefinitionId { get; set; }

        public bool CanStart { get; set; }

        public bool CanView { get; set; }

        public bool CanAction { get; set; }

        public bool CanAdminister { get; set; }

        public Role? Role { get; set; }

        public WorkflowDefinition? WorkflowDefinition { get; set; }
    }
}

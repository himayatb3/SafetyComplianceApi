namespace SafetyCompliance.Domain.Entity
{
    public class WorkflowDefinition : BaseEntity
    {
        public Guid ModuleId { get; set; }

        public Guid? CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; } = true;

        public Module? Module { get; set; }

        public Company? Company { get; set; }

        public ICollection<RoleWorkflowAccess> RoleAccesses { get; set; } = new List<RoleWorkflowAccess>();

        public ICollection<UserWorkflowAccessOverride> UserAccessOverrides { get; set; } = new List<UserWorkflowAccessOverride>();
    }
}

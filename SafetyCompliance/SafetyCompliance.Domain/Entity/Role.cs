namespace SafetyCompliance.Domain.Entity
{
    public class Role : BaseEntity
    {
        public Guid? CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsSystemRole { get; set; }

        public bool IsActive { get; set; } = true;

        public Company? Company { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<RoleMenuAccess> MenuAccesses { get; set; } = new List<RoleMenuAccess>();

        public ICollection<RoleWorkflowAccess> WorkflowAccesses { get; set; } = new List<RoleWorkflowAccess>();
    }
}

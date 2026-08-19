namespace SafetyCompliance.Domain.Entity
{
    public class MenuItem : BaseEntity
    {
        public Guid ModuleId { get; set; }

        public Guid? ParentMenuItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public Module? Module { get; set; }

        public MenuItem? ParentMenuItem { get; set; }

        public ICollection<MenuItem> ChildMenuItems { get; set; } = new List<MenuItem>();

        public ICollection<RoleMenuAccess> RoleAccesses { get; set; } = new List<RoleMenuAccess>();

        public ICollection<UserMenuAccessOverride> UserAccessOverrides { get; set; } = new List<UserMenuAccessOverride>();
    }
}

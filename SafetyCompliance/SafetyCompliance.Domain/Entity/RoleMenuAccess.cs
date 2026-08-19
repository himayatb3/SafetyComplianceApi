namespace SafetyCompliance.Domain.Entity
{
    public class RoleMenuAccess : BaseEntity
    {
        public Guid RoleId { get; set; }

        public Guid MenuItemId { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanApprove { get; set; }

        public bool CanExport { get; set; }

        public Role? Role { get; set; }

        public MenuItem? MenuItem { get; set; }
    }
}

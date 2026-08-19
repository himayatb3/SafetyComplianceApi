namespace SafetyCompliance.Domain.Entity
{
    public class UserMenuAccessOverride : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid MenuItemId { get; set; }

        public bool IsAllowed { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanApprove { get; set; }

        public bool CanExport { get; set; }

        public User? User { get; set; }

        public MenuItem? MenuItem { get; set; }
    }
}

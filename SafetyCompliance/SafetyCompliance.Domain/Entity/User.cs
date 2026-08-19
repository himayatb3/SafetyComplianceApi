namespace SafetyCompliance.Domain.Entity
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int Otp { get; set; }

        public int WrongLoginCount { get; set; }

        public bool IsBlocked { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int LoginCount { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public bool MustChangePassword { get; set; }

        public UserProfile? Profile { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<UserMenuAccessOverride> MenuAccessOverrides { get; set; } = new List<UserMenuAccessOverride>();

        public ICollection<UserWorkflowAccessOverride> WorkflowAccessOverrides { get; set; } = new List<UserWorkflowAccessOverride>();
    }
}

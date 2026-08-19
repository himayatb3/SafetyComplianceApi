namespace SafetyCompliance.Application.DTOs.Account
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }

        public Guid? SiteId { get; set; }

        public bool IsActive { get; set; }

        public bool MustChangePassword { get; set; }
    }
}

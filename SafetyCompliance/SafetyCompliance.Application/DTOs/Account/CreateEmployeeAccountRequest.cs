namespace SafetyCompliance.Application.DTOs.Account
{
    public class CreateEmployeeAccountRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string TemporaryPassword { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }

        public Guid? SiteId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public IReadOnlyCollection<Guid> RoleIds { get; set; } = Array.Empty<Guid>();

        public Guid CreatedBy { get; set; }
    }
}

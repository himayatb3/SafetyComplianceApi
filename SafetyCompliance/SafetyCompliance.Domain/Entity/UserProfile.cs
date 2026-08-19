namespace SafetyCompliance.Domain.Entity
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? SiteId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public User? User { get; set; }

        public Company? Company { get; set; }

        public Site? Site { get; set; }
    }
}

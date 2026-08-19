namespace SafetyCompliance.Domain.Entity
{
    public class Site : BaseEntity
    {
        public Guid CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Company? Company { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    }
}

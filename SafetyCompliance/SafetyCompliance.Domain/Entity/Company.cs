namespace SafetyCompliance.Domain.Entity
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<Site> Sites { get; set; } = new List<Site>();

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    }
}

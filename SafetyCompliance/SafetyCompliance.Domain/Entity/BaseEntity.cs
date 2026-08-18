namespace SafetyCompliance.Domain.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        
    }
}

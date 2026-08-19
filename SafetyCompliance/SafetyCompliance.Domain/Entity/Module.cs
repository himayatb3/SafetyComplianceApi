namespace SafetyCompliance.Domain.Entity
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public ICollection<WorkflowDefinition> Workflows { get; set; } = new List<WorkflowDefinition>();
    }
}

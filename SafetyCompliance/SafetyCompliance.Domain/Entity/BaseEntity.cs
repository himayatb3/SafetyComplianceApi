using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyCompliance.Domain.Entity
{
    public class BaseEntity
    {
        public Guid Id = Guid.NewGuid();

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}

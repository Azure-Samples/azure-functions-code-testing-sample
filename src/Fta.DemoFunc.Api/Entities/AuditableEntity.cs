using System;

namespace Fta.DemoFunc.Api.Entities
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }
}

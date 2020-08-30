using System;

namespace MiniURL.Domain.Common
{
    public abstract class AuditableEntity
    {
        // When using IdentityServer4 you could also include CraetedBy and ModifiedBy
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; } // Consider a way to recognize if an entity has not been modified.
    }
}
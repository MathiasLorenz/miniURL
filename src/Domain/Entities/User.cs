using System;
using System.Collections.Generic;
using System.Text;
using MiniURL.Domain.Common;

namespace MiniURL.Domain.Entities
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public HashSet<PersistedURL> PersistedURLs { get; set; } = new HashSet<PersistedURL>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MiniURL.Domain.Entities
{
    // Todo: Make AuditableEntity or something similar to handle createdDate etc.
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

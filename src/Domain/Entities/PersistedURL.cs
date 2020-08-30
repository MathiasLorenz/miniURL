using MiniURL.Domain.Common;

namespace MiniURL.Domain.Entities
{
    public class PersistedURL : AuditableEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string URL { get; set; } = null!;
        public string ShortURL { get; set; } = null!;
        public bool Deleted { get; set; } = false;

        public User? User { get; set; } = null;
    }
}

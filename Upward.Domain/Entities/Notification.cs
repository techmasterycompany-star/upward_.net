using Upwork.Domain.Common;
using Upwork.Domain.Enums;

namespace Upwork.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public long UserId { get; set; }

        public NotificationType Type { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Data { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public User User { get; set; } = null!;
    }
}

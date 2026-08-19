using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class JobView : BaseEntity
    {
        public long JobId { get; set; }

        public long? UserId { get; set; }

        public string? IpAddress { get; set; }

        public DateTime ViewedAt { get; set; }

        public Job Job { get; set; } = null!;

        public User? User { get; set; }
    }
}

using Upward.Domain.Common;
using Upward.Domain.Enums;

namespace Upward.Domain.Entities
{
    public class CommentReport : BaseEntity
    {
        public long CommentId { get; set; }

        public long UserId { get; set; }

        public string? Reason { get; set; }

        public CommentReportStatus Status { get; set; }

        public Comment Comment { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}

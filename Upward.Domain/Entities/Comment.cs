using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class Comment : SoftDeletableEntity
    {
        public long JobId { get; set; }

        public long UserId { get; set; }

        public string Content { get; set; } = null!;

        public bool IsApproved { get; set; }

        public Job Job { get; set; } = null!;

        public User User { get; set; } = null!;

        public ICollection<CommentReport> Reports { get; set; } = new List<CommentReport>();
    }
}

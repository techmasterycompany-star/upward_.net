using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class EmailVerificationToken : BaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        public User User { get; set; } = null!;
    }
}

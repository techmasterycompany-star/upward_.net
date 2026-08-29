using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        public User User { get; set; } = null!;
    }
}

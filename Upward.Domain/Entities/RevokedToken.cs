using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    // Stores the JWT ID (jti) of tokens invalidated via logout, until they'd have expired anyway

    public class RevokedToken : BaseEntity
    {
        public string Jti { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}

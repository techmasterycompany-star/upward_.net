using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IService
{
    public class GeneratedToken
    {
        public string AccessToken { get; set; } = null!;
        public string Jti { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
    public interface ITokenService
    {
        GeneratedToken GenerateAccessToken(User user);
    }
}

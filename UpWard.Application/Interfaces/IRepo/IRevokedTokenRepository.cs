namespace Upward.Application.Interfaces.IRepo
{
    public interface IRevokedTokenRepository
    {
        Task AddAsync(string jti, DateTime expiresAt);
        Task<bool> IsRevokedAsync(string jti);
    }
}

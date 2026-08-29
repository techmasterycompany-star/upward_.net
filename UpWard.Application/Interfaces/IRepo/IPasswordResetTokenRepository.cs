using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(string token);
        Task UpdateAsync(PasswordResetToken token);
    }
}

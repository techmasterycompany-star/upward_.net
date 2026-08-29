using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddAsync(EmailVerificationToken token);
        Task<EmailVerificationToken?> GetValidTokenAsync(string token);
        Task UpdateAsync(EmailVerificationToken token);
    }
}

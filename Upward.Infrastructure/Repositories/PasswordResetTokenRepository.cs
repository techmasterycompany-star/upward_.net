using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
        {
            private readonly AppDBContext _context;

            public PasswordResetTokenRepository(AppDBContext context) => _context = context;

            public async Task AddAsync(PasswordResetToken token)
            {
                await _context.PasswordResetTokens.AddAsync(token);
                await _context.SaveChangesAsync();
            }

            public Task<PasswordResetToken?> GetValidTokenAsync(string token) =>
                _context.PasswordResetTokens.FirstOrDefaultAsync(t =>
                    t.Token == token && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);

            public async Task UpdateAsync(PasswordResetToken token)
            {
                token.UpdatedAt = DateTime.UtcNow;
                _context.PasswordResetTokens.Update(token);
                await _context.SaveChangesAsync();
            }
        }
    
}
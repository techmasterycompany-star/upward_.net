using Microsoft.EntityFrameworkCore;
using System;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
        {
            private readonly AppDBContext _context;

            public EmailVerificationTokenRepository(AppDBContext context) => _context = context;

            public async Task AddAsync(EmailVerificationToken token)
            {
                await _context.EmailVerificationTokens.AddAsync(token);
                await _context.SaveChangesAsync();
            }

            public Task<EmailVerificationToken?> GetValidTokenAsync(string token) =>
                _context.EmailVerificationTokens.FirstOrDefaultAsync(t =>
                    t.Token == token && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);

            public async Task UpdateAsync(EmailVerificationToken token)
            {
                token.UpdatedAt = DateTime.UtcNow;
                _context.EmailVerificationTokens.Update(token);
                await _context.SaveChangesAsync();
            }
    }
    
}
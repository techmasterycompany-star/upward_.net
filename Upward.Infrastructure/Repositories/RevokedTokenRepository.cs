using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class RevokedTokenRepository : IRevokedTokenRepository
        {
            private readonly AppDBContext _context;

            public RevokedTokenRepository(AppDBContext context) => _context = context;

            public async Task AddAsync(string jti, DateTime expiresAt)
            {
                await _context.RevokedTokens.AddAsync(new RevokedToken
                {
                    Jti = jti,
                    ExpiresAt = expiresAt
                });
                await _context.SaveChangesAsync();
            }

            public Task<bool> IsRevokedAsync(string jti) =>
                _context.RevokedTokens.AnyAsync(t => t.Jti == jti);
        }
    
}
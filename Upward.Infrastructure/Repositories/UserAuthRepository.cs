using Microsoft.EntityFrameworkCore;
using System;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly AppDBContext _context;

        public UserAuthRepository(AppDBContext context) => _context = context;

        public Task<User?> GetByEmailAsync(string email) =>
            _context.Users
                .Include(u => u.EmployerProfile)
                .Include(u => u.CandidateProfile)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

        public Task<User?> GetByIdAsync(long id) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        public Task<bool> ExistsByEmailAsync(string email) =>
            _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
    
}
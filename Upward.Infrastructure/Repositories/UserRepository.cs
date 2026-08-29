using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Domain.Enums;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext context;
        public UserRepository(AppDBContext context) => this.context = context;

        public async Task ActivateUserAsync(User user)
        {
            user.IsSuspended = false;
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task<List<User>> FilterUsersAsync(bool? isSuspended, UserRole? role)
        {
           var query = context.Users.Where(u => !u.IsDeleted);

           if(role.HasValue) 
                query = query.Where(u => u.Role == role.Value);

           if(isSuspended.HasValue)
                query = query.Where(u => u.IsSuspended == isSuspended.Value);

           return await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync() => 
            await context.Users
            .Where(u => !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        public async Task<User?> GetByIdAsync(long id) => 
            await context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            var cleanKeyword = keyword.Trim().ToLower();
            return await context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Name.ToLower().Contains(cleanKeyword))
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task SuspendUserAsync(User user)
        {
            user.IsSuspended = true;
            await context.SaveChangesAsync();
        }
    }
}

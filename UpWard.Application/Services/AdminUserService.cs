using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository repo;
        public AdminUserService(IUserRepository repo) => this.repo = repo;

        public async Task<List<AdminUserDto>> GetUsersAsync(UserFilterDto? filter)
        {
            var users = string.IsNullOrWhiteSpace(filter?.Search)
                ? await repo.GetAllUsersAsync()
                : await repo.SearchUsersAsync(filter.Search.Trim());

            if (filter?.Role is not null)
                users = users.Where(u => u.Role == filter.Role.Value).ToList();

            if (filter?.IsSuspended is not null)
                users = users.Where(u => u.IsSuspended == filter.IsSuspended.Value).ToList();

            return users.Select(MapToDto).ToList();
        }

        public async Task<AdminUserDto?> GetUserAsync(long id)
        {
            var user = await repo.GetByIdAsync(id);
            if(user is null) return null;
            return MapToDto(user);
        }

        public async Task<bool> SuspendUserAsync(long id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user is null || user.Role == UserRole.Admin || user.IsSuspended)
                return false;

            await repo.SuspendUserAsync(user);
            return true;
        }

        public async Task<bool> ActivateUserAsync(long id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user is null || !user.IsSuspended)
                return false;

            await repo.ActivateUserAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user is null || user.Role == UserRole.Admin)
                return false;

            await repo.DeleteUserAsync(user);
            return true;
        }
        private static AdminUserDto MapToDto(User user) =>
           new()
           {
               Id = user.Id,
               Name = user.Name,
               Email = user.Email,
               Role = user.Role,
               IsSuspended = user.IsSuspended,
               IsEmailVerified = user.EmailVerifiedAt.HasValue,
               EmailVerifiedAt = user.EmailVerifiedAt,
               CreatedAt = user.CreatedAt
           };

    }
}

using Upwork.Domain.Entities;
using Upwork.Domain.Enums;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(long id);
        Task<List<User>> SearchUsersAsync(string? keyword);
        Task<List<User>> FilterUsersAsync(bool? isSuspended , UserRole? role);
        Task SuspendUserAsync(User user);
        Task ActivateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}

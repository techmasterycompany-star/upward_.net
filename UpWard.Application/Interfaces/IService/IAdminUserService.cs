using Upward.Application.DTOs.Admin;
using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IService
{
    public interface IAdminUserService
    {
        Task<List<AdminUserDto>> GetUsersAsync(UserFilterDto? filter);
        Task<AdminUserDto?> GetUserAsync(long id);
        Task<bool> SuspendUserAsync(long id);
        Task<bool> ActivateUserAsync(long id);
        Task<bool> DeleteUserAsync(long id);
    }
}

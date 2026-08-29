using Upwork.Domain.Enums;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IAdminDashboardRepository
    {
        Task<int> CountUsersAsync();
        Task<int> CountUsersByRoleAsync(UserRole role);
        Task<Dictionary<JobStatus, int>> GetJobCountsByStatusAsync();
        Task<int> CountApplicationsAsync();
        Task<int> CountCommentsAsync();
        Task<int> CountHiddenCommentsAsync();
    }
}

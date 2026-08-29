using Upwork.Domain.Enums;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IAdminDashboardRepository
    {
        Task<Dictionary<UserRole, int>> CountUsersByRoleAsync();
        Task<Dictionary<JobStatus, int>> GetJobCountsByStatusAsync();
        Task<int> CountApplicationsAsync();
        Task<int> CountCommentsAsync();
        Task<int> CountHiddenCommentsAsync();
    }
}

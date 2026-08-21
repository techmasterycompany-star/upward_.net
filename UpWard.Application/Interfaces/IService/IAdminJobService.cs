using Upward.Application.DTOs.Admin;

namespace Upward.Application.Interfaces.IService
{
    public interface IAdminJobService
    {
        Task<List<AdminJobDto>> GetJobsAsync();
        Task<List<AdminJobDto>> GetPendingJobsAsync();
        Task<AdminJobDto?> GetJobAsync(long id);
        Task ApproveJobAsync(long id);
        Task RejectJobAsync(long id, string reason);
    }
}

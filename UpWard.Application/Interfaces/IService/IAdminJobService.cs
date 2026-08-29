using Upwork.Application.DTOs.Admin;

namespace Upwork.Application.Interfaces.IService
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

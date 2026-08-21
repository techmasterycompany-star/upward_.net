using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IJobRepository
    {
        Task<List<Job>> GetAllJobsAsync();
        Task<List<Job>> GetPendingJobsAsync();
        Task<Job?> GetJobByIdAsync(long id);
        Task ApproveJobAsync(Job job);
        Task RejectJobAsync(Job job);
    }
}

using Upward.Application.DTOs.Employer;
using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IEmployerApplicationRepository
    {
        Task<List<JobApplication>> GetByJobIdAsync(long jobId);
        Task<List<JobApplication>> GetByEmployerIdAsync(long employerId);
        Task<JobApplication?> GetByIdAsync(long id);
        void Update(JobApplication application);
        Task<bool> ExistsByJobEmployerAsync(long applicationId, long employerId);
    }
}

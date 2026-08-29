using Upwork.Application.DTOs.Employer;
using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
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

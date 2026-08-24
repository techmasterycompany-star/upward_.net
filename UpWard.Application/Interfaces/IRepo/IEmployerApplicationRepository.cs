using Upward.Application.DTOs.Employer;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IEmployerApplicationRepository
    {
        Task<List<Upward.Domain.Entities.Application>> GetByJobIdAsync(long jobId);
        Task<List<Upward.Domain.Entities.Application>> GetByEmployerIdAsync(long employerId);
        Task<Upward.Domain.Entities.Application?> GetByIdAsync(long id);
        void Update(Upward.Domain.Entities.Application application);
        Task<bool> ExistsByJobEmployerAsync(long applicationId, long employerId);
    }
}

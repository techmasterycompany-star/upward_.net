using Upward.Application.DTOs.Employer;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IEmployerJobRepository
    {
        Task<Upward.Domain.Entities.Job?> GetByIdAsync(long id);
        Task<List<Upward.Domain.Entities.Job>> GetByEmployerIdAsync(long employerId);
        Task<Upward.Domain.Entities.Job> CreateAsync(Upward.Domain.Entities.Job job);
        void Update(Upward.Domain.Entities.Job job);
        void Delete(Upward.Domain.Entities.Job job);
        Task<bool> ExistsByEmployerIdAsync(long employerId, long jobId);
    }
}

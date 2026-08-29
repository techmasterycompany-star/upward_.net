using Upwork.Application.DTOs.Employer;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IEmployerJobRepository
    {
        Task<Upwork.Domain.Entities.Job?> GetByIdAsync(long id);
        Task<List<Upwork.Domain.Entities.Job>> GetByEmployerIdAsync(long employerId);
        Task<Upwork.Domain.Entities.Job> CreateAsync(Upwork.Domain.Entities.Job job);
        void Update(Upwork.Domain.Entities.Job job);
        void Delete(Upwork.Domain.Entities.Job job);
        Task<bool> ExistsByEmployerIdAsync(long employerId, long jobId);
    }
}

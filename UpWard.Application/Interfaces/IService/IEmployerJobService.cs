using Upward.Application.DTOs.Employer;

namespace Upward.Application.Interfaces.IService
{
    public interface IEmployerJobService
    {
        Task<List<JobDetailDto>> GetJobsByEmployerAsync(long employerId);
        Task<JobDetailDto?> GetByIdAsync(long id, long employerId);
        Task<JobDetailDto> CreateAsync(long employerId, CreateJobRequest request);
        Task<JobDetailDto> UpdateAsync(long id, long employerId, UpdateJobRequest request);
        Task<bool> DeleteAsync(long id, long employerId);
        Task<bool> CloseAsync(long id, long employerId);
    }
}

using Upwork.Application.DTOs.Employer;

namespace Upwork.Application.Interfaces.IService
{
    public interface IEmployerApplicationService
    {
        Task<List<ApplicationDto>> GetApplicationsByJobAsync(long jobId, long employerId);
        Task<List<ApplicationDto>> GetApplicationsByEmployerAsync(long employerId);
        Task<ApplicationDto?> GetByIdAsync(long id, long employerId);
        Task<ApplicationDto> AcceptAsync(long id, long employerId);
        Task<ApplicationDto> RejectAsync(long id, long employerId, ReviewApplicationRequest request);
    }
}

using Upward.Application.DTOs.Employer;

namespace Upward.Application.Interfaces.IService
{
    public interface IEmployerService
    {
        Task<EmployerProfileDto?> GetByIdAsync(long id);
        Task<EmployerProfileDto?> GetByUserIdAsync(long userId);
        Task<List<EmployerProfileDto>> GetAllAsync();
        Task<List<EmployerProfileDto>> SearchAsync(string? keyword);
        Task<EmployerProfileDto> CreateAsync(CreateEmployerProfileRequest request);
        Task<EmployerProfileDto> UpdateAsync(long id, UpdateEmployerProfileRequest request);
        Task<bool> DeleteAsync(long id);
        Task<List<EmployerJobDto>> GetJobsAsync(long employerId);
        Task<string> UploadLogoAsync(long employerId, Stream fileStream, string fileName);
    }
}

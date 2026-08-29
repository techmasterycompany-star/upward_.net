using Upwork.Application.DTOs.Employer;

namespace Upwork.Application.Interfaces.IService
{
    public interface IEmployerAnalyticsService
    {
        Task<EmployerDashboardDto> GetDashboardAsync(long employerId);
        Task<List<JobAnalyticsDto>> GetJobAnalyticsAsync(long employerId);
        Task<List<CandidateSearchDto>> SearchCandidatesAsync(string? keyword, List<string>? skills);
    }
}

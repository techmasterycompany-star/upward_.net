using Upwork.Application.DTOs.Employer;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IEmployerAnalyticsRepository
    {
        Task<List<Upwork.Domain.Entities.Job>> GetJobsWithStatsAsync(long employerId);
        Task<List<Upwork.Domain.Entities.CandidateProfile>> SearchCandidatesAsync(string? keyword, List<string>? skills);
    }
}

using Upward.Application.DTOs.Employer;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IEmployerAnalyticsRepository
    {
        Task<List<Upward.Domain.Entities.Job>> GetJobsWithStatsAsync(long employerId);
        Task<List<Upward.Domain.Entities.CandidateProfile>> SearchCandidatesAsync(string? keyword, List<string>? skills);
    }
}

using Upward.Application.DTOs.Employer;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;

namespace Upward.Application.Services
{
    public class EmployerAnalyticsService : IEmployerAnalyticsService
    {
        private readonly IEmployerAnalyticsRepository _analyticsRepository;

        public EmployerAnalyticsService(IEmployerAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<EmployerDashboardDto> GetDashboardAsync(long employerId)
        {
            var jobs = await _analyticsRepository.GetJobsWithStatsAsync(employerId);

            return new EmployerDashboardDto
            {
                TotalJobs = jobs.Count,
                ActiveJobs = jobs.Count(j => j.Status == Domain.Enums.JobStatus.Approved),
                TotalApplications = jobs.Sum(j => j.Applications?.Count ?? 0),
                PendingApplications = jobs.Sum(j => j.Applications?.Count(a =>
                    a.Status == Domain.Enums.ApplicationStatus.Submitted ||
                    a.Status == Domain.Enums.ApplicationStatus.UnderReview) ?? 0),
                AcceptedApplications = jobs.Sum(j => j.Applications?.Count(a =>
                    a.Status == Domain.Enums.ApplicationStatus.Accepted) ?? 0),
                TotalViews = jobs.Sum(j => j.ViewsCount),
                TopJobs = jobs.Take(5).Select(j => new JobAnalyticsDto
                {
                    JobId = j.Id,
                    JobTitle = j.Title,
                    ViewsCount = j.ViewsCount,
                    ApplicationsCount = j.Applications?.Count ?? 0,
                    AcceptedCount = j.Applications?.Count(a => a.Status == Domain.Enums.ApplicationStatus.Accepted) ?? 0,
                    RejectedCount = j.Applications?.Count(a => a.Status == Domain.Enums.ApplicationStatus.Rejected) ?? 0,
                    PendingCount = j.Applications?.Count(a =>
                        a.Status == Domain.Enums.ApplicationStatus.Submitted ||
                        a.Status == Domain.Enums.ApplicationStatus.UnderReview) ?? 0,
                    CreatedAt = j.CreatedAt
                }).ToList()
            };
        }

        public async Task<List<JobAnalyticsDto>> GetJobAnalyticsAsync(long employerId)
        {
            var jobs = await _analyticsRepository.GetJobsWithStatsAsync(employerId);
            return jobs.Select(j => new JobAnalyticsDto
            {
                JobId = j.Id,
                JobTitle = j.Title,
                ViewsCount = j.ViewsCount,
                ApplicationsCount = j.Applications?.Count ?? 0,
                AcceptedCount = j.Applications?.Count(a => a.Status == Domain.Enums.ApplicationStatus.Accepted) ?? 0,
                RejectedCount = j.Applications?.Count(a => a.Status == Domain.Enums.ApplicationStatus.Rejected) ?? 0,
                PendingCount = j.Applications?.Count(a =>
                    a.Status == Domain.Enums.ApplicationStatus.Submitted ||
                    a.Status == Domain.Enums.ApplicationStatus.UnderReview) ?? 0,
                CreatedAt = j.CreatedAt
            }).ToList();
        }

        public async Task<List<CandidateSearchDto>> SearchCandidatesAsync(string? keyword, List<string>? skills)
        {
            var candidates = await _analyticsRepository.SearchCandidatesAsync(keyword, skills);
            return candidates.Select(c => new CandidateSearchDto
            {
                CandidateId = c.Id,
                Name = c.User?.Name ?? "",
                Email = c.User?.Email ?? "",
                Headline = c.Headline,
                Location = c.Location,
                Resume = c.Resume,
                Skills = c.CandidateSkills?.Select(cs => cs.Skill.Name).ToList() ?? new(),
                IsDiscoverable = c.IsDiscoverable
            }).ToList();
        }
    }
}

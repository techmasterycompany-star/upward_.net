using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class EmployerAnalyticsRepository : IEmployerAnalyticsRepository
    {
        private readonly AppDBContext context;
        public EmployerAnalyticsRepository(AppDBContext context) => this.context = context;

        public async Task<List<Job>> GetJobsWithStatsAsync(long employerId) =>
            await context.Jobs
            .Include(j => j.Applications)
            .Where(j => j.EmployerId == employerId && !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        public async Task<List<CandidateProfile>> SearchCandidatesAsync(string? keyword, List<string>? skills) =>
            await context.CandidateProfiles
            .Include(c => c.User)
            .Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Where(c => c.IsDiscoverable &&
                (string.IsNullOrWhiteSpace(keyword) ||
                 c.User.Name.Contains(keyword) ||
                 (c.Headline != null && c.Headline.Contains(keyword)) ||
                 (c.Location != null && c.Location.Contains(keyword))) &&
                (skills == null || !skills.Any() ||
                 c.CandidateSkills.Any(cs => skills.Contains(cs.Skill.Name))))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}

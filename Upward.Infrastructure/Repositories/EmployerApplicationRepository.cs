using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class EmployerApplicationRepository : IEmployerApplicationRepository
    {
        private readonly AppDBContext context;
        public EmployerApplicationRepository(AppDBContext context) => this.context = context;

        public async Task<List<JobApplication>> GetByJobIdAsync(long jobId) =>
            await context.Applications
            .Include(a => a.Candidate).ThenInclude(c => c.User)
            .Include(a => a.Candidate).ThenInclude(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Include(a => a.Job)
            .Where(a => a.JobId == jobId && !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        public async Task<List<JobApplication>> GetByEmployerIdAsync(long employerId) =>
            await context.Applications
            .Include(a => a.Candidate).ThenInclude(c => c.User)
            .Include(a => a.Candidate).ThenInclude(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Include(a => a.Job)
            .Where(a => a.Job.EmployerId == employerId && !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        public async Task<JobApplication?> GetByIdAsync(long id) =>
            await context.Applications
            .Include(a => a.Candidate).ThenInclude(c => c.User)
            .Include(a => a.Candidate).ThenInclude(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        public void Update(JobApplication application)
        {
            context.Applications.Update(application);
        }

        public async Task<bool> ExistsByJobEmployerAsync(long applicationId, long employerId) =>
            await context.Applications.AnyAsync(a =>
                a.Id == applicationId &&
                a.Job.EmployerId == employerId &&
                !a.IsDeleted);
    }
}

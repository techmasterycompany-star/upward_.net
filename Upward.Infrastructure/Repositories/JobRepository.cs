using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Domain.Enums;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDBContext context;
        public JobRepository(AppDBContext context) => this.context = context;


        public async Task<List<Job>> GetAllJobsAsync() => 
            await context.Jobs
                .Include(j => j.Employer)
                .Include(j => j.Category)
                .Where(j => !j.IsDeleted)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        public async Task<List<Job>> GetPendingJobsAsync() => 
            await context.Jobs
                .Include(j => j.Employer)
                .Include(j => j.Category)
                .Where(j => !j.IsDeleted && j.Status == JobStatus.PendingApproval)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        public async Task<Job?> GetJobByIdAsync(long id) => 
            await context.Jobs
                .Include(j => j.Employer)
                .Include(j => j.Category)
                .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);
        public async Task ApproveJobAsync(Job job)
        {
            job.Status = JobStatus.Approved;
            job.RejectionReason = null;
            await context.SaveChangesAsync();
        }
        public async Task RejectJobAsync(Job job)
        {
            job.Status = JobStatus.Rejected;
            await context.SaveChangesAsync();
        }
    }
}

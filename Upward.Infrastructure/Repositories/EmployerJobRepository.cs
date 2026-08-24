using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class EmployerJobRepository : IEmployerJobRepository
    {
        private readonly AppDBContext context;
        public EmployerJobRepository(AppDBContext context) => this.context = context;

        public async Task<Job?> GetByIdAsync(long id) =>
            await context.Jobs
            .Include(j => j.Employer)
            .Include(j => j.Category)
            .Include(j => j.JobTechnologies).ThenInclude(jt => jt.Technology)
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);

        public async Task<List<Job>> GetByEmployerIdAsync(long employerId) =>
            await context.Jobs
            .Include(j => j.Category)
            .Include(j => j.JobTechnologies).ThenInclude(jt => jt.Technology)
            .Where(j => j.EmployerId == employerId && !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        public async Task<Job> CreateAsync(Job job)
        {
            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();
            return job;
        }

        public void Update(Job job)
        {
            context.Jobs.Update(job);
        }

        public void Delete(Job job)
        {
            context.Jobs.Remove(job);
        }

        public async Task<bool> ExistsByEmployerIdAsync(long employerId, long jobId) =>
            await context.Jobs.AnyAsync(j => j.Id == jobId && j.EmployerId == employerId && !j.IsDeleted);
    }
}

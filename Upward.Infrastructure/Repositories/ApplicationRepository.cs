using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Domain.Enums;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDBContext _context;

        public ApplicationRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(JobApplication application)
        {
            await _context.Applications.AddAsync(application);
        }

        public async Task<bool> ExistsAsync(long jobId, long candidateId)
        {
            return await _context.Applications.AnyAsync(x => x.JobId == jobId && x.CandidateId == candidateId);
        }

        public async Task<bool> ExistsNotCancelledAsync(long jobId, long candidateId)
        {
            return await _context.Applications.AnyAsync(x => x.JobId == jobId && x.CandidateId == candidateId && x.Status != ApplicationStatus.Cancelled);
        }

        public async Task<JobApplication?> GetByIdAsync(long applicationId, long candidateId)
        {
            return await _context.Applications
                .Include(x => x.Job)
                .FirstOrDefaultAsync(x => x.Id == applicationId && x.CandidateId == candidateId);
        }

        public async Task<List<JobApplication>> GetByCandidateIdAsync(long candidateId)
        {
            return await _context.Applications
                .Include(x => x.Job)
                .Where(x => x.CandidateId == candidateId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public void Update(JobApplication application)
        {
            _context.Applications.Update(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

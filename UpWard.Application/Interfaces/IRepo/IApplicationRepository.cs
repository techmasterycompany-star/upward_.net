using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IApplicationRepository
    {
        Task AddAsync(JobApplication application);
        Task<bool> ExistsAsync(long jobId, long candidateId);
        Task<bool> ExistsNotCancelledAsync(long jobId, long candidateId);
        Task<JobApplication?> GetByIdAsync(long applicationId, long candidateId);
        Task<List<JobApplication>> GetByCandidateIdAsync(long candidateId);
        void Update(JobApplication application);
        Task SaveChangesAsync();
    }
}

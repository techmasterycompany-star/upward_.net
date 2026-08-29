using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Application.DTOs.Common;
using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IJobRepository
    {
        // Admin
        Task<List<Job>> GetAllJobsAsync();
        Task<List<Job>> GetPendingJobsAsync();
        Task<Job?> GetJobByIdAsync(long id);
        Task ApproveJobAsync(Job job);
        Task RejectJobAsync(Job job);

        // Job Search / Details
        Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request);
        Task<Job?> GetApprovedJobByIdAsync(long jobId);

        // Job Views
        Task<JobView?> GetExistingViewAsync(long jobId, long? userId, string? ipAddress);
        Task AddJobViewAsync(JobView jobView);
        Task IncrementViewsCountAsync(long jobId);

        // Saved Searches
        Task AddSavedSearchAsync(SavedSearch savedSearch);
        Task<List<SavedSearch>> GetSavedSearchesAsync(long candidateId);
        Task<SavedSearch?> GetSavedSearchByIdAsync(long candidateId, long savedSearchId);
        void RemoveSavedSearch(SavedSearch savedSearch);

        Task SaveChangesAsync();
    }
}

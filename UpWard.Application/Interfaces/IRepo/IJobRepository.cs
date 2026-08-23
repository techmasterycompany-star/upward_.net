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
        Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request);
        Task<Job?> GetByIdAsync(long jobId);
        Task<JobView?> GetExistingViewAsync(long jobId, long? userId, string? ipAddress);
        Task AddJobViewAsync(JobView jobView);
        Task IncrementViewsCountAsync(long jobId);
        Task AddSavedSearchAsync(SavedSearch savedSearch);
        Task<List<SavedSearch>> GetSavedSearchesAsync(long candidateId);
        Task<SavedSearch?> GetSavedSearchByIdAsync(long candidateId, long savedSearchId);
        void RemoveSavedSearch(SavedSearch savedSearch);
        Task SaveChangesAsync();
    }
}

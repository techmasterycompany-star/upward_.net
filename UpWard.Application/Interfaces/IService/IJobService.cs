using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upwork.Application.DTOs.Common;

namespace Upwork.Application.Interfaces.IService
{
    public interface IJobService
    {
        Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request);
        Task<JobDetailsDto> GetByIdAsync(long jobId);
        Task RecordViewAsync(long jobId, long? userId, string? ipAddress);
        Task<SavedSearchDto> SaveSearchAsync(long candidateId, SaveJobSearchRequestDto request);
        Task<List<SavedSearchDto>> GetSavedSearchesAsync(long candidateId);
        Task DeleteSavedSearchAsync(long candidateId, long savedSearchId);
    }
}

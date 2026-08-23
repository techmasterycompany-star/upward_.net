using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;

namespace Upward.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request)
        {
            ValidateRequest(request);

            return await _jobRepository.SearchAsync(request);            
        }

        private static void ValidateRequest(JobSearchRequestDto request)
        {
            if (request.MinSalary.HasValue &&request.MaxSalary.HasValue && request.MinSalary > request.MaxSalary)
            {
                throw new ArgumentException( "Minimum salary cannot be greater than maximum salary.");
            }

            if (request.Page < 1) request.Page = 1;           
            if (request.PageSize < 1) request.PageSize = 10;         
            if (request.PageSize > 100) request.PageSize = 100;
            
        }

        public async Task<JobDetailsDto> GetByIdAsync(long jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found.");

            return new JobDetailsDto
            {
                Id = job.Id,
                EmployerId = job.EmployerId,
                CategoryId = job.CategoryId,
                CategoryName = job.Category.Name,
                Title = job.Title,
                Description = job.Description,
                Responsibilities = job.Responsibilities,
                Requirements = job.Requirements,
                Location = job.Location,
                WorkType = job.WorkType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                ExperienceLevel = job.ExperienceLevel,
                ApplicationDeadline = job.ApplicationDeadline,
                CreatedAt = job.CreatedAt,
                ViewsCount = job.ViewsCount,
                ApplicationsCount = job.ApplicationsCount
            };
        }

        public async Task RecordViewAsync(long jobId, long? userId, string? ipAddress)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found.");

            var existingView = await _jobRepository.GetExistingViewAsync(jobId, userId, ipAddress);

            if (existingView != null)
                return;

            var jobView = new JobView
            {
                JobId = jobId,
                UserId = userId,
                IpAddress = ipAddress,
                ViewedAt = DateTime.Now
            };

            await _jobRepository.AddJobViewAsync(jobView);
            await _jobRepository.IncrementViewsCountAsync(jobId);
            await _jobRepository.SaveChangesAsync();
        }

        public async Task<SavedSearchDto> SaveSearchAsync(long candidateId, SaveJobSearchRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Search name is required.");

            if (request.MinSalary.HasValue &&
                request.MaxSalary.HasValue &&
                request.MinSalary > request.MaxSalary)
            {
                throw new ArgumentException("Minimum salary cannot be greater than maximum salary.");
            }

            var savedSearch = new SavedSearch
            {
                CandidateId = candidateId,
                Name = request.Name.Trim(),
                Keyword = Normalize(request.Keyword),
                Location = Normalize(request.Location),
                CategoryId = request.CategoryId,
                WorkType = request.WorkType,
                MinSalary = request.MinSalary,
                MaxSalary = request.MaxSalary,
                ExperienceLevel = request.ExperienceLevel
            };

            await _jobRepository.AddSavedSearchAsync(savedSearch);
            await _jobRepository.SaveChangesAsync();

            return MapSavedSearch(savedSearch);
        }

        public async Task<List<SavedSearchDto>> GetSavedSearchesAsync(long candidateId)
        {
            var searches = await _jobRepository.GetSavedSearchesAsync(candidateId);

            return searches.Select(MapSavedSearch).ToList();
        }

        public async Task DeleteSavedSearchAsync(long candidateId, long savedSearchId)
        {
            var savedSearch = await _jobRepository.GetSavedSearchByIdAsync(candidateId, savedSearchId);

            if (savedSearch == null)
                throw new KeyNotFoundException("Saved search not found.");

            _jobRepository.RemoveSavedSearch(savedSearch);

            await _jobRepository.SaveChangesAsync();
        }

        private static string? Normalize(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static SavedSearchDto MapSavedSearch(SavedSearch search)
        {
            return new SavedSearchDto
            {
                Id = search.Id,
                Name = search.Name,
                Keyword = search.Keyword,
                Location = search.Location,
                CategoryId = search.CategoryId,
                WorkType = search.WorkType,
                MinSalary = search.MinSalary,
                MaxSalary = search.MaxSalary,
                ExperienceLevel = search.ExperienceLevel
            };
        }
    }
}

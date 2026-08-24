using Microsoft.EntityFrameworkCore;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Mappings;
using Upward.Domain.Entities;
using Upward.Domain.Enums;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDBContext _context;

        public JobRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request)
        {
            IQueryable<Job> query = _context.Jobs
                .AsNoTracking()
                .Include(j => j.Employer)
                .Where(j =>!j.IsDeleted && (j.Status == JobStatus.Approved || j.Status == JobStatus.Closed));

            // Keyword
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();

                query = query.Where(j => j.Title.Contains(keyword) || j.Description.Contains(keyword));
            }

            // Location
            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                var location = request.Location.Trim();

                query = query.Where(j => j.Location != null &&  j.Location.Contains(location));
            }

            // Category
            if (request.CategoryId.HasValue)
            {
                query = query.Where(j => j.CategoryId == request.CategoryId.Value);
            }

            // Work Type
            if (request.WorkType.HasValue)
            {
                query = query.Where(j => j.WorkType == request.WorkType.Value);
            }

            // Salary
            if (request.MinSalary.HasValue)
            {
                query = query.Where(j => j.SalaryMax.HasValue && j.SalaryMax.Value >= request.MinSalary.Value);
            }

            if (request.MaxSalary.HasValue)
            {
                query = query.Where(j => j.SalaryMin.HasValue && j.SalaryMin.Value <= request.MaxSalary.Value);
            }

            // Experience Level
            if (request.ExperienceLevel.HasValue)
            {
                query = query.Where(j => j.ExperienceLevel == request.ExperienceLevel.Value);
            }

            // Date Posted
            if (request.PostedAfter.HasValue && request.PostedAfter.Value != DatePostedFilter.AnyTime)
            {
                var date = GetPostedAfterDate(request.PostedAfter.Value);

                query = query.Where(j => j.CreatedAt >= date);
            }

            // Total count
            var totalCount = await query.CountAsync();

            // Sorting
            query = ApplySorting(query, request);

            // Pagination
            var page = request.Page < 1? 1 : request.Page;

            var pageSize = request.PageSize <= 0? 10 : Math.Min(request.PageSize, 100);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(j => j.ToSearchResultDto())
                .ToListAsync();

            return new PagedResultDto<JobSearchResultDto>
            {
                Items = items,
                PageSize = pageSize,
                Page = page,
                TotalCount = totalCount,
            };
        }

        private static IQueryable<Job> ApplySorting(IQueryable<Job> query, JobSearchRequestDto request)
        {
            return request.SortBy switch
            {
                JobSortBy.Salary => request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(j => j.SalaryMin) : query.OrderByDescending(j => j.SalaryMin),

                JobSortBy.DatePosted => request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(j => j.CreatedAt) : query.OrderByDescending(j => j.CreatedAt),

                _ => query.OrderByDescending(j => j.CreatedAt)
            };
        }

        private static DateTime GetPostedAfterDate(DatePostedFilter filter)
        {
            var now = DateTime.Now;

            return filter switch
            {
                DatePostedFilter.Today =>  now.Date,
                DatePostedFilter.Last3Days => now.AddDays(-3),
                DatePostedFilter.Last7Days => now.AddDays(-7),
                DatePostedFilter.Last30Days => now.AddDays(-30),
                _ => DateTime.MinValue
            };
        }

        public async Task<Job?> GetByIdAsync(long jobId)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == jobId && !j.IsDeleted && j.Status == JobStatus.Approved);
        }

        public async Task<JobView?> GetExistingViewAsync(long jobId, long? userId, string? ipAddress)
        {
            if (userId.HasValue)
            {
                return await _context.JobViews
                    .FirstOrDefaultAsync(x =>
                        x.JobId == jobId &&
                        x.UserId == userId);
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
                return null;

            return await _context.JobViews
                .FirstOrDefaultAsync(x =>
                    x.JobId == jobId &&
                    x.UserId == null &&
                    x.IpAddress == ipAddress);
        }

        public async Task AddJobViewAsync(JobView jobView)
        {
            await _context.JobViews.AddAsync(jobView);
        }

        public async Task IncrementViewsCountAsync(long jobId)
        {
            await _context.Jobs
                .Where(j => j.Id == jobId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(j => j.ViewsCount, j => j.ViewsCount + 1));
        }

        public async Task AddSavedSearchAsync(SavedSearch savedSearch)
        {
            await _context.SavedSearches.AddAsync(savedSearch);
        }

        public async Task<List<SavedSearch>> GetSavedSearchesAsync(long candidateId)
        {
            return await _context.SavedSearches
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.CandidateId == candidateId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<SavedSearch?> GetSavedSearchByIdAsync(long candidateId, long savedSearchId)
        {
            return await _context.SavedSearches
                .FirstOrDefaultAsync(x => x.Id == savedSearchId && x.CandidateId == candidateId);
        }

        public void RemoveSavedSearch(SavedSearch savedSearch)
        {
            _context.SavedSearches.Remove(savedSearch);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

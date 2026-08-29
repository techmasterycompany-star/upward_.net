using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Common
{
    public class JobSearchRequestDto
    {
        // Search
        public string? Keyword { get; set; }

        // Filters
        public string? Location { get; set; }

        public long? CategoryId { get; set; }

        public WorkType? WorkType { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public ExperienceLevel? ExperienceLevel { get; set; }
        //public bool? isClosed { get; set; }

        public DatePostedFilter? PostedAfter { get; set; } = DatePostedFilter.AnyTime;

        // Sorting
        public JobSortBy SortBy { get; set; } = JobSortBy.DatePosted;

        public SortDirection SortDirection { get; set; }
            = SortDirection.Descending;

        // Pagination
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}

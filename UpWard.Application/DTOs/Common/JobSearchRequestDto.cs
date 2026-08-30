using System.ComponentModel.DataAnnotations;
using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Common
{
    public class JobSearchRequestDto
    {
        // Search
        [StringLength(100)]
        public string? Keyword { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Range(1, long.MaxValue)]
        public long? CategoryId { get; set; }

        public WorkType? WorkType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxSalary { get; set; }

        public ExperienceLevel? ExperienceLevel { get; set; }

        public DatePostedFilter? PostedAfter { get; set; } = DatePostedFilter.AnyTime;
        //public bool? isClosed { get; set; }

        // Sorting
        public JobSortBy SortBy { get; set; } = JobSortBy.DatePosted;

        public SortDirection SortDirection { get; set; } = SortDirection.Descending;

        // Pagination
        [Range(1, 1000)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}

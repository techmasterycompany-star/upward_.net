using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Common
{
    public class JobSearchResultDto
    {
        public long Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Location { get; set; }

        public WorkType WorkType { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public ExperienceLevel ExperienceLevel { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public long CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public long EmployerId { get; set; }
        public string? CompanyName { get; set; }
    }


}

using Upward.Domain.Enums;

namespace Upward.Application.DTOs.Admin
{
    public class AdminJobDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string? Location { get; set; }
        public WorkType WorkType { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public JobStatus Status { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public int ViewsCount { get; set; }
        public int ApplicationsCount { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

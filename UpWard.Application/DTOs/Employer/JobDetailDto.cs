namespace Upwork.Application.DTOs.Employer
{
    public class JobDetailDto
    {
        public long Id { get; set; }
        public long EmployerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Responsibilities { get; set; } = null!;
        public string Requirements { get; set; } = null!;
        public string? Location { get; set; }
        public string WorkType { get; set; } = null!;
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string ExperienceLevel { get; set; } = null!;
        public DateTime ApplicationDeadline { get; set; }
        public string Status { get; set; } = null!;
        public string? RejectionReason { get; set; }
        public int ViewsCount { get; set; }
        public int ApplicationsCount { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<string> Technologies { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

namespace Upwork.Application.DTOs.Employer
{
    public class EmployerJobDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public string WorkType { get; set; } = null!;
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string ExperienceLevel { get; set; } = null!;
        public DateTime ApplicationDeadline { get; set; }
        public string Status { get; set; } = null!;
        public int ViewsCount { get; set; }
        public int ApplicationsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

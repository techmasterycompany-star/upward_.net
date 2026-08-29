namespace Upwork.Application.DTOs.Employer
{
    public class CreateJobRequest
    {
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
        public long CategoryId { get; set; }
        public List<long>? TechnologyIds { get; set; }
    }
}

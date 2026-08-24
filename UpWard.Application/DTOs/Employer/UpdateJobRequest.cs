namespace Upward.Application.DTOs.Employer
{
    public class UpdateJobRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Responsibilities { get; set; }
        public string? Requirements { get; set; }
        public string? Benefits { get; set; }
        public string? Location { get; set; }
        public string? WorkType { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? ExperienceLevel { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public long? CategoryId { get; set; }
        public List<long>? TechnologyIds { get; set; }
    }
}

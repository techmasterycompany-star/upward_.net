using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Common
{
    public class SaveJobSearchRequestDto
    {
        public string Name { get; set; } = null!;

        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public long? CategoryId { get; set; }

        public WorkType? WorkType { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public ExperienceLevel? ExperienceLevel { get; set; }
    }
}

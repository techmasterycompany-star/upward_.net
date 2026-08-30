using System.ComponentModel.DataAnnotations;
using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Common
{
    public class SaveJobSearchRequestDto
    {
        [Required]
        [StringLength(100,  MinimumLength = 1, ErrorMessage = "Search name must be between 1 and 100 characters.")]
        public string Name { get; set; } = null!;

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
    }
}

using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Candidate
{
    public class CandidateSkillInputDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 50)]
        public int YearsExperience { get; set; }
    }
    public class CandidateSkillsCsvInputDto
    {
        [Required]
        [StringLength(5000, MinimumLength = 1)]
        public string skills { get; set; } = string.Empty;

        [Range(0, 50)]
        public int YearsExperience { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Candidate
{
    public class UpdateCandidateSkillsDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one skill is required.")]
        [MaxLength(50, ErrorMessage = "You can add up to 50 skills.")]
        public List<CandidateSkillInputDto> Skills { get; set; } = [];
    }
}

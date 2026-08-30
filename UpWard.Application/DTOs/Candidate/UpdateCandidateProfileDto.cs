using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Candidate
{
    public class UpdateCandidateProfileDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = null!;
        [StringLength(150)]
        public string? Headline { get; set; }

        [StringLength(2000)]
        public string? Bio { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Url]
        [StringLength(500)]
        public string? PortfolioUrl { get; set; }

        public IFormFile? ResumeFile { get; set; }

        [Url]
        [StringLength(500)]
        public string? LinkedinProfile { get; set; }

        public bool IsDiscoverable { get; set; }
    }
}

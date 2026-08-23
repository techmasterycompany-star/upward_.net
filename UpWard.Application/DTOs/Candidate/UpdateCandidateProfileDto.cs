using Microsoft.AspNetCore.Http;

namespace Upward.Application.DTOs.Candidate
{
    public class UpdateCandidateProfileDto
    {
        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public string? Location { get; set; }

        public string? PortfolioUrl { get; set; }

        public IFormFile? ResumeFile { get; set; }

        public string? LinkedinProfile { get; set; }

        public bool IsDiscoverable { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upward.Application.DTOs.Candidate
{
    public class CandidateProfileDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Headline { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? PortfolioUrl { get; set; }

        public string? ResumeUrl { get; set; }

        public string? LinkedinProfile { get; set; }

        public bool IsDiscoverable { get; set; }

        public List<CandidateSkillDto> Skills { get; set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}

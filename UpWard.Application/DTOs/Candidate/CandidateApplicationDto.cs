using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Candidate
{
    public class CandidateApplicationDto
    {
        public long Id { get; set; }

        public long JobId { get; set; }

        public string JobTitle { get; set; } = null!;

        public ApplicationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public string? RejectionReason { get; set; }
    }


}

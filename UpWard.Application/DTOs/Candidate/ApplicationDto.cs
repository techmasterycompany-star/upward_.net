using Upward.Domain.Enums;

namespace Upward.Application.DTOs.Candidate
{
    public class ApplicationDto
    {
        public long Id { get; set; }

        public long JobId { get; set; }

        public string JobTitle { get; set; } = null!;

        public long CandidateId { get; set; }

        public string Resume { get; set; } = null!;

        public string? CoverLetter { get; set; }

        public string? Message { get; set; }

        public string ContactEmail { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public ApplicationStatus Status { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; }
    }


}

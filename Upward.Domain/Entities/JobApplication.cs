using Upward.Domain.Common;
using Upward.Domain.Enums;

namespace Upward.Domain.Entities
{
    
    public class JobApplication : SoftDeletableEntity
    {
        public long JobId { get; set; }

        public long CandidateId { get; set; }

        public string Resume { get; set; } = null!;

        public string? CoverLetter { get; set; }

        public string? Message { get; set; }

        public string ContactEmail { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        public DateTime? ReviewedAt { get; set; }

        public string? RejectionReason { get; set; }

        public bool AppliedViaLinkedIn { get; set; }

        // Navigation properties
        public Job Job { get; set; } = null!;

        public CandidateProfile Candidate { get; set; } = null!;
    }
}

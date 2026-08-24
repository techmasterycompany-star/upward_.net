namespace Upward.Application.DTOs.Employer
{
    public class ApplicationDto
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public long CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public string CandidateEmail { get; set; } = null!;
        public string? CandidateHeadline { get; set; }
        public string? CandidateLocation { get; set; }
        public string? CandidateResume { get; set; }
        public string? CandidateLinkedin { get; set; }
        public List<string> CandidateSkills { get; set; } = new();
        public string? CoverLetter { get; set; }
        public string? Message { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? RejectionReason { get; set; }
        public bool AppliedViaLinkedIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

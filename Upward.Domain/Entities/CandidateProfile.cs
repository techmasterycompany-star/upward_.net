using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class CandidateProfile : BaseEntity
    {
        public long UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public string? Location { get; set; }

        public string? PortfolioUrl { get; set; }

        public string? ResumeUrl { get; set; }
        public string? ResumePublicId { get; set; }

        public string? LinkedinProfile { get; set; }

        public bool IsDiscoverable { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public ICollection<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();
    }
}

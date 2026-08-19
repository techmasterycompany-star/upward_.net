using Upward.Domain.Common;
using Upward.Domain.Enums;

namespace Upward.Domain.Entities
{
    public class SavedSearch : BaseEntity
    {
        public long CandidateId { get; set; }

        public string Name { get; set; } = null!;

        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public long? CategoryId { get; set; }

        public WorkType? WorkType { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public ExperienceLevel? ExperienceLevel { get; set; }

        public CandidateProfile Candidate { get; set; } = null!;

        public Category? Category { get; set; }
    }
}

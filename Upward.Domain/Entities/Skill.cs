using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Category { get; set; }

        public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    }
}

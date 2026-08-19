using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class CandidateSkill : BaseEntity
    {
        public long CandidateProfileId { get; set; }

        public long SkillId { get; set; }

        public int YearsExperience { get; set; }

        public CandidateProfile CandidateProfile { get; set; } = null!;

        public Skill Skill { get; set; } = null!;
    }
}

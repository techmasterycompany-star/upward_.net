namespace Upwork.Application.DTOs.Candidate
{
    public class CandidateSkillDto
    {
        public long Id { get; set; }
        public long SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? Category { get; set; }
        public int YearsExperience { get; set; }
    }


}

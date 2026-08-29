using Upwork.Application.DTOs.Candidate;
using Upwork.Domain.Entities;

namespace Upwork.Application.Mappings
{
    public static class CandidateProfileMapper
    {
        public static CandidateProfileDto ToDto(this CandidateProfile profile)
        {
            return new CandidateProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Name = profile.User.Name,
                Email = profile.User.Email,
                IsDeleted = profile.User.IsDeleted,
                CreatedAt = profile.User.CreatedAt,
                UpdatedAt = profile.User.UpdatedAt,
                Headline = profile.Headline,
                Bio = profile.Bio,
                Location = profile.Location,
                PortfolioUrl = profile.PortfolioUrl,
                ResumeUrl = profile.ResumeUrl,
                LinkedinProfile = profile.LinkedinProfile,
                IsDiscoverable = profile.IsDiscoverable,
                Skills = profile.CandidateSkills
                    .Where(x => x.Skill is not null)
                    .Select(x => new CandidateSkillDto
                    {
                        Id = x.Id,
                        SkillId = x.SkillId,
                        SkillName = x.Skill!.Name,
                        Category = x.Skill.Category,
                        YearsExperience = x.YearsExperience
                    })
                    .OrderBy(x => x.SkillName)
                    .ToList()
            };
        }
    }
}
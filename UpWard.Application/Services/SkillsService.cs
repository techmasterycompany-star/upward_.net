using Upward.Application.DTOs.Candidate;
using Upward.Application.Mappings;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;

namespace Upward.Application.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly ICandidateProfileRepository _candidateProfileRepository;
        private readonly ISkillsRepository _skillsRepository;

        public SkillsService(
            ICandidateProfileRepository candidateProfileRepository,
            ISkillsRepository skillsRepository)
        {
            _candidateProfileRepository = candidateProfileRepository;
            _skillsRepository = skillsRepository;
        }

        public async Task<CandidateProfileDto?> AddSkillAsync(long userId, CandidateSkillInputDto request)
        {
            return await UpsertSkillsAsync(userId, new[] { request }, removeMissing: false);
        }

        public Task<CandidateProfileDto?> AddSkillsBulkAsync(long userId, UpdateCandidateSkills request)
        {
            var skills = ParseSkillsCsv(request.Skills)
                .Select(name => new CandidateSkillInputDto
                {
                    Name = name,
                    YearsExperience = request.YearsExperience
                });

            return UpsertSkillsAsync(userId, skills, removeMissing: false);
        }

        public async Task<CandidateProfileDto?> UpdateSkillAsync(long userId, long candidateSkillId, CandidateSkillInputDto request)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
            {
                return null;
            }

            var candidateSkill = profile.CandidateSkills.FirstOrDefault(x => x.Id == candidateSkillId);

            if (candidateSkill is null)
            {
                return null;
            }

            var normalizedName = NormalizeSkillName(request.Name);
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                throw new ArgumentException("Skill name is required.", nameof(request));
            }

            var yearsExperience = request.YearsExperience < 0 ? 0 : request.YearsExperience;
            var targetSkill = await _skillsRepository.GetByNameAsync(normalizedName);

            if (targetSkill is null)
            {
                targetSkill = new Skill { Name = normalizedName };
                await _skillsRepository.AddAsync(targetSkill);
            }

            var duplicateRelation = profile.CandidateSkills.FirstOrDefault(x => x.Id != candidateSkillId && x.Skill is not null && string.Equals(x.Skill.Name, normalizedName, StringComparison.OrdinalIgnoreCase));

            if (duplicateRelation is not null)
            {
                duplicateRelation.YearsExperience = yearsExperience;
                profile.CandidateSkills.Remove(candidateSkill);
            }
            else
            {
                candidateSkill.Skill = targetSkill;
                candidateSkill.SkillId = targetSkill.Id;
                candidateSkill.YearsExperience = yearsExperience;
            }

            profile.UpdatedAt = DateTime.UtcNow;
            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        public async Task<CandidateProfileDto?> RemoveSkillAsync(long userId, long candidateSkillId)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
            {
                return null;
            }

            var candidateSkill = profile.CandidateSkills.FirstOrDefault(x => x.Id == candidateSkillId);

            if (candidateSkill is null)
            {
                return null;
            }

            profile.CandidateSkills.Remove(candidateSkill);
            profile.UpdatedAt = DateTime.UtcNow;

            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        public Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkills request)
        {
            var skills = ParseSkillsCsv(request.Skills)
                .Select(name => new CandidateSkillInputDto
                {
                    Name = name,
                    YearsExperience = request.YearsExperience
                });

            return UpsertSkillsAsync(userId, skills, removeMissing: true);
        }

        public Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkillsDto request)
        {
            return UpsertSkillsAsync(userId, request.Skills, removeMissing: true);
        }

        private async Task<CandidateProfileDto?> UpsertSkillsAsync(long userId, IEnumerable<CandidateSkillInputDto> requestSkills, bool removeMissing)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
            {
                return null;
            }

            var desiredSkills = NormalizeSkills(requestSkills);
            var desiredNames = desiredSkills.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingByName = profile.CandidateSkills
                .Where(x => x.Skill is not null)
                .ToDictionary(x => NormalizeSkillName(x.Skill!.Name), StringComparer.OrdinalIgnoreCase);

            if (removeMissing)
            {
                var toRemove = profile.CandidateSkills
                    .Where(x => x.Skill is not null && !desiredNames.Contains(NormalizeSkillName(x.Skill!.Name)))
                    .ToList();

                foreach (var candidateSkill in toRemove)
                {
                    profile.CandidateSkills.Remove(candidateSkill);
                }
            }

            foreach (var (skillName, input) in desiredSkills)
            {
                if (existingByName.TryGetValue(skillName, out var existingRelation))
                {
                    existingRelation.YearsExperience = input.YearsExperience;
                    continue;
                }

                var skill = await _skillsRepository.GetByNameAsync(skillName);

                if (skill is null)
                {
                    skill = new Skill { Name = skillName };
                    await _skillsRepository.AddAsync(skill);
                }

                profile.CandidateSkills.Add(new CandidateSkill
                {
                    CandidateProfileId = profile.Id,
                    Skill = skill,
                    YearsExperience = input.YearsExperience
                });
            }

            profile.UpdatedAt = DateTime.UtcNow;
            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        private static Dictionary<string, CandidateSkillInputDto> NormalizeSkills(IEnumerable<CandidateSkillInputDto> skills)
        {
            var normalized = new Dictionary<string, CandidateSkillInputDto>(StringComparer.OrdinalIgnoreCase);

            foreach (var skill in skills)
            {
                var name = NormalizeSkillName(skill.Name);

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                normalized[name] = new CandidateSkillInputDto
                {
                    Name = name,
                    YearsExperience = skill.YearsExperience < 0 ? 0 : skill.YearsExperience
                };
            }

            return normalized;
        }

        private static IEnumerable<string> ParseSkillsCsv(string skills)
        {
            return skills
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(NormalizeSkillName)
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static string NormalizeSkillName(string name)
        {
            return name.Trim();
        }

    }
}

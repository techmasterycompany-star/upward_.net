using Upwork.Application.DTOs.Candidate;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Application.Mappings;
using Upwork.Domain.Entities;

namespace Upwork.Application.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly ICandidateProfileRepository _candidateProfileRepository;
        private readonly ISkillsRepository _skillsRepository;

        public SkillsService(ICandidateProfileRepository candidateProfileRepository, ISkillsRepository skillsRepository)
        {
            _candidateProfileRepository = candidateProfileRepository;
            _skillsRepository = skillsRepository;
        }
        public async Task<IEnumerable<CandidateSkillDto>?> GetSkillsAsync(long userId)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
                return null;

            return profile.CandidateSkills
                .Where(x => x.Skill is not null)
                .Select(x => new CandidateSkillDto
                {
                    Id = x.Id,
                    SkillId = x.SkillId,
                    SkillName = x.Skill!.Name,
                    Category = x.Skill.Category,
                    YearsExperience = x.YearsExperience
                })
                .ToList();
        }
        public async Task<CandidateProfileDto?> AddSkillAsync(long userId,CandidateSkillInputDto request)
        {
            ValidateSkillInput(request);


            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
                return null;

            var normalizedName = NormalizeSkillName(request.Name);

            // Prevent adding the same skill twice
            var existingCandidateSkill = profile.CandidateSkills
                .FirstOrDefault(x =>
                    x.Skill is not null &&
                    string.Equals(NormalizeSkillName(x.Skill.Name), normalizedName, StringComparison.OrdinalIgnoreCase));

            if (existingCandidateSkill is not null)
            {
                existingCandidateSkill.YearsExperience = request.YearsExperience;
            }
            else
            {
                var skill = await _skillsRepository.GetByNameAsync(normalizedName);

                if (skill is null)
                {
                    skill = new Skill
                    {
                        Name = normalizedName
                    };

                    await _skillsRepository.AddAsync(skill);
                }

                profile.CandidateSkills.Add(new CandidateSkill
                {
                    CandidateProfileId = profile.Id,
                    Skill = skill,
                    YearsExperience = request.YearsExperience
                });
            }

            profile.UpdatedAt = DateTime.UtcNow;

            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        public async Task<CandidateProfileDto?> AddSkillsCsvAsync(long userId, CandidateSkillsCsvInputDto request)
        {
            ValidateSkillInput(request);

            var skills = request.skills
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (skills.Count == 0)
                throw new ArgumentException("At least one skill is required.");

            if(skills.Count > 50)
                throw new ArgumentException("You can add up to 50 skills.");


            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
                return null;

            foreach (var skillName in skills)
            {
                var normalizedName = NormalizeSkillName(skillName);

                // Check if candidate already has this skill
                var existingCandidateSkill = profile.CandidateSkills
                    .FirstOrDefault(x =>
                        x.Skill is not null &&
                        string.Equals(
                            NormalizeSkillName(x.Skill.Name),
                            normalizedName,
                            StringComparison.OrdinalIgnoreCase));

                if (existingCandidateSkill is not null)
                {
                    existingCandidateSkill.YearsExperience = request.YearsExperience;
                    continue;
                }

                // Check if skill already exists globally
                var skill = await _skillsRepository.GetByNameAsync(normalizedName);

                if (skill is null)
                {
                    skill = new Skill
                    {
                        Name = normalizedName
                    };

                    await _skillsRepository.AddAsync(skill);
                }

                // Add candidate-skill relationship
                profile.CandidateSkills.Add(new CandidateSkill
                {
                    CandidateProfileId = profile.Id,
                    Skill = skill,
                    YearsExperience = request.YearsExperience
                });
            }

            profile.UpdatedAt = DateTime.UtcNow;

            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        public async Task<CandidateProfileDto?> UpdateSkillAsync(long userId, long candidateSkillId, CandidateSkillInputDto request)
        {
            ValidateSkillInput(request);

            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
                return null;

            var candidateSkill = profile.CandidateSkills
                .FirstOrDefault(x => x.Id == candidateSkillId);

            if (candidateSkill is null)
                return null;

            var normalizedName = NormalizeSkillName(request.Name);

            var targetSkill = await _skillsRepository.GetByNameAsync(normalizedName);

            if (targetSkill is null)
            {
                targetSkill = new Skill
                {
                    Name = normalizedName
                };

                await _skillsRepository.AddAsync(targetSkill);
            }

            // Check whether another CandidateSkill already uses this skill
            var duplicateRelation = profile.CandidateSkills
                .FirstOrDefault(x =>
                    x.Id != candidateSkillId &&
                    x.Skill is not null &&
                    string.Equals(
                        NormalizeSkillName(x.Skill.Name),
                        normalizedName,
                        StringComparison.OrdinalIgnoreCase));

            if (duplicateRelation is not null)
            {
                duplicateRelation.YearsExperience = request.YearsExperience;

                profile.CandidateSkills.Remove(candidateSkill);
            }
            else
            {
                candidateSkill.Skill = targetSkill;
                candidateSkill.SkillId = targetSkill.Id;
                candidateSkill.YearsExperience = request.YearsExperience;
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
                return null;

            var candidateSkill = profile.CandidateSkills
                .FirstOrDefault(x => x.Id == candidateSkillId);

            if (candidateSkill is null)
                return null;

            profile.CandidateSkills.Remove(candidateSkill);
            profile.UpdatedAt = DateTime.UtcNow;

            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        public async Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkillsDto request)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
                return null;

            var desiredSkills = NormalizeSkills(request.Skills);

            // Remove skills that are not included in the new list
            var skillsToRemove = profile.CandidateSkills
                .Where(x =>
                    x.Skill is not null &&
                    !desiredSkills.ContainsKey(
                        NormalizeSkillName(x.Skill.Name)))
                .ToList();

            foreach (var candidateSkill in skillsToRemove)
            {
                profile.CandidateSkills.Remove(candidateSkill);
            }

            // Add/update requested skills
            foreach (var (skillName, input) in desiredSkills)
            {
                var existingCandidateSkill = profile.CandidateSkills
                    .FirstOrDefault(x =>
                        x.Skill is not null &&
                        string.Equals(
                            NormalizeSkillName(x.Skill.Name),
                            skillName,
                            StringComparison.OrdinalIgnoreCase));

                if (existingCandidateSkill is not null)
                {
                    existingCandidateSkill.YearsExperience =
                        input.YearsExperience;

                    continue;
                }

                var skill = await _skillsRepository.GetByNameAsync(skillName);

                if (skill is null)
                {
                    skill = new Skill
                    {
                        Name = skillName
                    };

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
            var normalized =
                new Dictionary<string, CandidateSkillInputDto>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (var skill in skills)
            {
                var name = NormalizeSkillName(skill.Name);

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                if (skill.YearsExperience < 0)
                    throw new ArgumentException(
                        "Years of experience cannot be negative.");

                normalized[name] = new CandidateSkillInputDto
                {
                    Name = name,
                    YearsExperience = skill.YearsExperience
                };
            }

            return normalized;
        }

        private static void ValidateSkillInput(CandidateSkillInputDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Skill name is required.");

            if (request.YearsExperience < 0)
                throw new ArgumentException("Years of experience cannot be negative.");

            if (request.Name.Contains(','))
                throw new ArgumentException("Skill name cannot contain commas.");
        }
        private static void ValidateSkillInput(CandidateSkillsCsvInputDto request)
        {
            if (string.IsNullOrWhiteSpace(request.skills))
                throw new ArgumentException("Skills is required.");

            if (request.YearsExperience < 0)
                throw new ArgumentException("Years of experience cannot be negative.");
        }

        private static string NormalizeSkillName(string name)
        {
            return name.Trim();
        }
    }
}


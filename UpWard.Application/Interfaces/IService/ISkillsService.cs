using Upward.Application.DTOs.Candidate;

namespace Upward.Application.Interfaces.IService
{
    public interface ISkillsService
    {
        Task<CandidateProfileDto?> AddSkillAsync(long userId, CandidateSkillInputDto request);

        Task<CandidateProfileDto?> AddSkillsBulkAsync(long userId, UpdateCandidateSkills request);

        Task<CandidateProfileDto?> UpdateSkillAsync(long userId, long candidateSkillId, CandidateSkillInputDto request);

        Task<CandidateProfileDto?> RemoveSkillAsync(long userId, long candidateSkillId);

        Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkills request);

        Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkillsDto request);
    }
}

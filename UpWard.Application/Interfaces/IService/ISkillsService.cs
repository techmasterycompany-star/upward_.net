using Upwork.Application.DTOs.Candidate;

namespace Upwork.Application.Interfaces.IService
{
    public interface ISkillsService
    {
        Task<IEnumerable<CandidateSkillDto>?> GetSkillsAsync(long userId);
        Task<CandidateProfileDto?> AddSkillAsync(long userId, CandidateSkillInputDto request);
        Task<CandidateProfileDto?> AddSkillsCsvAsync(long userId, CandidateSkillsCsvInputDto request);
        Task<CandidateProfileDto?> UpdateSkillAsync(long userId, long candidateSkillId, CandidateSkillInputDto request);
        Task<CandidateProfileDto?> RemoveSkillAsync(long userId, long candidateSkillId);
        Task<CandidateProfileDto?> UpdateSkillsAsync(long userId, UpdateCandidateSkillsDto request);
    }
}

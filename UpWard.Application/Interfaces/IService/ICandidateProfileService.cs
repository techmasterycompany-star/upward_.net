using Upward.Application.DTOs.Candidate;

namespace Upward.Application.Interfaces.IService
{
    public interface ICandidateProfileService
    {
        Task<CandidateProfileDto> CreateAsync(long userId, UpdateCandidateProfileDto dto);

        Task<CandidateProfileDto?> GetByUserIdAsync(long userId);

        Task<CandidateProfileDto?> UpdateAsync(long userId, UpdateCandidateProfileDto dto);
    }
}

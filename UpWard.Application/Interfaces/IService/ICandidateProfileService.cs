using Microsoft.AspNetCore.Http;
using Upwork.Application.DTOs.Candidate;

namespace Upwork.Application.Interfaces.IService
{
    public interface ICandidateProfileService
    {
        Task<CandidateProfileDto> CreateAsync(long userId, UpdateCandidateProfileDto dto);
        Task<CandidateProfileDto?> UploadResumeAsync(long userId, IFormFile resumeFile);

        Task<CandidateProfileDto?> GetByUserIdAsync(long userId);

        Task<CandidateProfileDto?> UpdateAsync(long userId, UpdateCandidateProfileDto dto);
    }
}

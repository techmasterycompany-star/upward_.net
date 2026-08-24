using Microsoft.AspNetCore.Http;
using Upward.Application.DTOs.Candidate;
using Upward.Application.Mappings;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Application.Validators;
using Upward.Domain.Entities;

namespace Upward.Application.Services
{
    public class CandidateProfileService : ICandidateProfileService
    {
        private readonly ICandidateProfileRepository _candidateProfileRepository;
        private readonly IStorageService _storageService;

        public CandidateProfileService(
            ICandidateProfileRepository candidateProfileRepository,
            IStorageService storageService)
        {
            _candidateProfileRepository = candidateProfileRepository;
            _storageService = storageService;
        }

        public async Task<CandidateProfileDto> CreateAsync(long userId, UpdateCandidateProfileDto dto)
        {
            if (await _candidateProfileRepository.ExistsAsync(userId))
            {
                throw new InvalidOperationException("Candidate profile already exists.");
            }

            var resume = await UploadResumeAsync(dto.ResumeFile, required: true);

            var profile = new CandidateProfile
            {
                UserId = userId,
                Headline = dto.Headline,
                Bio = dto.Bio,
                Location = dto.Location,
                PortfolioUrl = dto.PortfolioUrl,
                ResumeUrl = resume.Url,
                ResumePublicId = resume.PublicId,
                LinkedinProfile = dto.LinkedinProfile,
                IsDiscoverable = dto.IsDiscoverable
            };

            await _candidateProfileRepository.AddAsync(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return await GetByUserIdOrThrowAsync(userId);
        }

        public async Task<CandidateProfileDto?> GetByUserIdAsync(long userId)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);
            return profile is null ? null : profile.ToDto();
        }

        public async Task<CandidateProfileDto?> UpdateAsync(long userId, UpdateCandidateProfileDto dto)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId);

            if (profile is null)
            {
                return null;
            }

            profile.Headline = dto.Headline;
            profile.Bio = dto.Bio;
            profile.Location = dto.Location;
            profile.PortfolioUrl = dto.PortfolioUrl;
            if (dto.ResumeFile is not null)
            {
                var resume = await UploadResumeAsync(dto.ResumeFile, required: false);
                profile.ResumeUrl = resume.Url;
                profile.ResumePublicId = resume.PublicId;
            }
            profile.LinkedinProfile = dto.LinkedinProfile;
            profile.IsDiscoverable = dto.IsDiscoverable;
            profile.UpdatedAt = DateTime.UtcNow;

            _candidateProfileRepository.Update(profile);
            await _candidateProfileRepository.SaveChangesAsync();

            return profile.ToDto();
        }

        private async Task<(string Url, string PublicId)> UploadResumeAsync(IFormFile? resumeFile, bool required)
        {
            if (resumeFile is null)
            {
                if (required)
                {
                    throw new ArgumentException("Resume file is required.", nameof(resumeFile));
                }

                return (string.Empty, string.Empty);
            }

            using var stream = resumeFile.OpenReadStream();
            var validationResult = await ResumeFileValidator.ValidateAsync(new ResumeFileDto
            {
                Content = stream,
                FileName = resumeFile.FileName,
                ContentType = resumeFile.ContentType,
                Length = resumeFile.Length
            });

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ErrorMesssage ?? "Invalid resume file.");
            }

            var uploadResult = await _storageService.UploadAsync(stream, resumeFile.FileName, resumeFile.ContentType, "resumes");
            var url = string.IsNullOrWhiteSpace(uploadResult.SecureUrl) ? uploadResult.Url : uploadResult.SecureUrl;

            return (url, uploadResult.PublicId);
        }

        private async Task<CandidateProfileDto> GetByUserIdOrThrowAsync(long userId)
        {
            var profile = await _candidateProfileRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Candidate profile was not created.");

            return profile.ToDto();
        }
    }
}

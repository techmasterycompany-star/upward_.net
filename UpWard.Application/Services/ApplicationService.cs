using Microsoft.AspNetCore.Http;
using Upward.Application.DTOs.Candidate;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Application.Mappings;
using Upward.Application.Validators;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IStorageService _storageService;

        public ApplicationService(IApplicationRepository applicationRepository, IJobRepository jobRepository, IStorageService storageService)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _storageService = storageService;
        }

        public async Task<ApplicationDto> ApplyAsync(long candidateId, long jobId, ApplyJobRequestDto request)
        {
            if (!request.ConfirmContactInformation)
            {
                throw new ArgumentException("You must confirm your contact information.");
            }

            if (string.IsNullOrWhiteSpace(request.ContactEmail))
            {
                throw new ArgumentException("Contact email is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ContactPhone))
            {
                throw new ArgumentException("Contact phone is required.");
            }

            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job is null)
            {
                throw new KeyNotFoundException("Job not found.");
            }

            if (job.Status != JobStatus.Approved)
            {
                throw new InvalidOperationException("You can only apply to approved jobs.");
            }

            var alreadyApplied = await _applicationRepository.ExistsNotCancelledAsync(jobId, candidateId);

            if (alreadyApplied)
            {
                throw new InvalidOperationException("You have already applied to this job.");
            }

            var resume = await UploadResumeAsync(request.Resume);

            var application = new JobApplication
            {
                JobId = jobId,
                CandidateId = candidateId,
                Resume = resume.Url,
                CoverLetter = Normalize(request.CoverLetter),
                Message = Normalize(request.Message),
                ContactEmail = request.ContactEmail.Trim(),
                ContactPhone = request.ContactPhone.Trim(),
                Status = ApplicationStatus.Submitted,
                AppliedViaLinkedIn = false
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            application.Job = job;

            return application.ToDto();
        }

        public async Task<List<CandidateApplicationDto>> GetMyApplicationsAsync(long candidateId)
        {
            var applications = await _applicationRepository.GetByCandidateIdAsync(candidateId);

            return applications
                .Select(a => a.ToCandidateApplicationDto())
                .ToList();
        }

        public async Task CancelAsync(long candidateId, long applicationId)
        {
            var application = await _applicationRepository
                .GetByIdAsync(applicationId, candidateId);

            if (application is null)
            {
                throw new KeyNotFoundException("Application not found.");
            }

            if (application.Status != ApplicationStatus.Submitted && application.Status != ApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException("Application cannot be cancelled after a final decision.");
            }

            application.Status = ApplicationStatus.Cancelled;

            _applicationRepository.Update(application);

            await _applicationRepository.SaveChangesAsync();
        }

        private async Task<(string Url, string PublicId)> UploadResumeAsync(IFormFile resumeFile)
        {
            if (resumeFile is null)
            {
                throw new ArgumentException("Resume file is required.");
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

            var uploadResult = await _storageService.UploadAsync(
                stream,
                resumeFile.FileName,
                resumeFile.ContentType,
                "applications/resumes");

            var url = string.IsNullOrWhiteSpace(uploadResult.SecureUrl)? uploadResult.Url : uploadResult.SecureUrl;

            return (url, uploadResult.PublicId);
        }

        private static string? Normalize(string? value)
        {
            return string.IsNullOrWhiteSpace(value)? null : value.Trim();
        }


    }
}

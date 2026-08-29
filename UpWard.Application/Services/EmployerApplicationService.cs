using Upwork.Application.DTOs.Employer;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;
using Upwork.Domain.Enums;

namespace Upwork.Application.Services
{
    public class EmployerApplicationService : IEmployerApplicationService
    {
        private readonly IEmployerApplicationRepository _applicationRepository;
        private readonly INotificationService _notificationService;

        public EmployerApplicationService(IEmployerApplicationRepository applicationRepository, INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _notificationService = notificationService;
        }

        public async Task<List<ApplicationDto>> GetApplicationsByJobAsync(long jobId, long employerId)
        {
            var applications = await _applicationRepository.GetByJobIdAsync(jobId);
            return applications.Select(a => MapToDto(a)).ToList();
        }

        public async Task<List<ApplicationDto>> GetApplicationsByEmployerAsync(long employerId)
        {
            var applications = await _applicationRepository.GetByEmployerIdAsync(employerId);
            return applications.Select(a => MapToDto(a)).ToList();
        }

        public async Task<ApplicationDto?> GetByIdAsync(long id, long employerId)
        {
            var exists = await _applicationRepository.ExistsByJobEmployerAsync(id, employerId);
            if (!exists) return null;

            var application = await _applicationRepository.GetByIdAsync(id);
            return application == null ? null : MapToDto(application);
        }

        public async Task<ApplicationDto> AcceptAsync(long id, long employerId)
        {
            var application = await _applicationRepository.GetByIdAsync(id)
                ?? throw new Exception("Application not found.");

            if (application.Job.EmployerId != employerId)
                throw new Exception("You don't have permission to review this application.");

            if (application.Status != ApplicationStatus.Submitted && application.Status != ApplicationStatus.UnderReview)
                throw new Exception("Application cannot be accepted in its current status.");

            application.Status = ApplicationStatus.Accepted;
            application.ReviewedAt = DateTime.UtcNow;
            _applicationRepository.Update(application);

            await _notificationService.NotifyApplicationAcceptedAsync(application.Candidate.UserId, application.Job.Title);

            return MapToDto(application);
        }

        public async Task<ApplicationDto> RejectAsync(long id, long employerId, ReviewApplicationRequest request)
        {
            var application = await _applicationRepository.GetByIdAsync(id)
                ?? throw new Exception("Application not found.");

            if (application.Job.EmployerId != employerId)
                throw new Exception("You don't have permission to review this application.");

            if (application.Status != ApplicationStatus.Submitted && application.Status != ApplicationStatus.UnderReview)
                throw new Exception("Application cannot be rejected in its current status.");

            application.Status = ApplicationStatus.Rejected;
            application.RejectionReason = request.RejectionReason;
            application.ReviewedAt = DateTime.UtcNow;
            _applicationRepository.Update(application);

            await _notificationService.NotifyApplicationRejectedAsync(application.Candidate.UserId, application.Job.Title, request.RejectionReason);

            return MapToDto(application);
        }

        private static ApplicationDto MapToDto(JobApplication a) => new()
        {
            Id = a.Id,
            JobId = a.JobId,
            JobTitle = a.Job?.Title ?? "",
            CandidateId = a.CandidateId,
            CandidateName = a.Candidate?.User?.Name ?? "",
            CandidateEmail = a.Candidate?.User?.Email ?? "",
            CandidateHeadline = a.Candidate?.Headline,
            CandidateLocation = a.Candidate?.Location,
            CandidateResume = a.Candidate?.ResumeUrl,
            CandidateLinkedin = a.Candidate?.LinkedinProfile,
            CandidateSkills = a.Candidate?.CandidateSkills?.Select(cs => cs.Skill.Name).ToList() ?? new(),
            CoverLetter = a.CoverLetter,
            Message = a.Message,
            ContactEmail = a.ContactEmail,
            ContactPhone = a.ContactPhone,
            Status = a.Status.ToString(),
            RejectionReason = a.RejectionReason,
            AppliedViaLinkedIn = a.AppliedViaLinkedIn,
            CreatedAt = a.CreatedAt
        };
    }
}

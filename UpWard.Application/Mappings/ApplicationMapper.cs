using Upwork.Application.DTOs.Candidate;
using Upwork.Domain.Entities;

namespace Upwork.Application.Mappings
{
    public static class ApplicationMapper
    {
        public static ApplicationDto ToDto(this JobApplication application)
        {
            return new ApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = application.Job.Title,
                CandidateId = application.CandidateId,
                Resume = application.Resume,
                CoverLetter = application.CoverLetter,
                Message = application.Message,
                ContactEmail = application.ContactEmail,
                ContactPhone = application.ContactPhone,
                Status = application.Status,
                ReviewedAt = application.ReviewedAt,
                RejectionReason = application.RejectionReason,
                CreatedAt = application.CreatedAt
            };
        }

        public static CandidateApplicationDto ToCandidateApplicationDto(this JobApplication application)
        {
            return new CandidateApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = application.Job.Title,
                Status = application.Status,
                CreatedAt = application.CreatedAt,
                ReviewedAt = application.ReviewedAt,
                RejectionReason = application.RejectionReason
            };
        }

    }
}

using Upwork.Application.DTOs.Admin;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;
using Upwork.Domain.Enums;

namespace Upwork.Application.Services
{
    public class AdminJobService : IAdminJobService
    {
        private readonly IJobRepository repo;
        public AdminJobService(IJobRepository repo) => this.repo = repo;

        public async Task<List<AdminJobDto>> GetJobsAsync()
        {
            var jobs = await repo.GetAllJobsAsync();
            return jobs.Select(MapToDto).ToList();
        }

        public async Task<List<AdminJobDto>> GetPendingJobsAsync()
        {
            var jobs = await repo.GetPendingJobsAsync();
            return jobs.Select(MapToDto).ToList();
        }

        public async Task<AdminJobDto?> GetJobAsync(long id)
        {
            var job = await repo.GetJobByIdAsync(id);
            return job is null ? null : MapToDto(job);
        }

        public async Task ApproveJobAsync(long id)
        {
            var job = await repo.GetJobByIdAsync(id);
            if (job is null)
                throw new KeyNotFoundException($"Job with id {id} was not found.");

            await repo.ApproveJobAsync(job);
        }

        public async Task RejectJobAsync(long id, string reason)
        {
            var job = await repo.GetJobByIdAsync(id);
            if (job is null || job.Status == JobStatus.Rejected)
                throw new KeyNotFoundException($"Job with id {id} was not found.");

            job.RejectionReason = string.IsNullOrWhiteSpace(reason) ? "No reason provided." : reason.Trim();
            await repo.RejectJobAsync(job);
        }

        private static AdminJobDto MapToDto(Job job) =>
           new()
           {
               Id = job.Id,
               Title = job.Title,
               CompanyName = job.Employer.CompanyName,
               CategoryName = job.Category.Name,
               Location = job.Location,
               WorkType = job.WorkType,
               ExperienceLevel = job.ExperienceLevel,
               Status = job.Status,
               SalaryMin = job.SalaryMin,
               SalaryMax = job.SalaryMax,
               ViewsCount = job.ViewsCount,
               ApplicationsCount = job.ApplicationsCount,
               ApplicationDeadline = job.ApplicationDeadline,
               CreatedAt = job.CreatedAt
           };
    }
}

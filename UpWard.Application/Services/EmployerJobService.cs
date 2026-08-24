using Upward.Application.DTOs.Employer;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class EmployerJobService : IEmployerJobService
    {
        private readonly IEmployerJobRepository _jobRepository;
        private readonly IEmployerRepository _employerRepository;

        public EmployerJobService(IEmployerJobRepository jobRepository, IEmployerRepository employerRepository)
        {
            _jobRepository = jobRepository;
            _employerRepository = employerRepository;
        }

        public async Task<List<JobDetailDto>> GetJobsByEmployerAsync(long employerId)
        {
            var jobs = await _jobRepository.GetByEmployerIdAsync(employerId);
            return jobs.Select(MapToDto).ToList();
        }

        public async Task<JobDetailDto?> GetByIdAsync(long id, long employerId)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null || job.EmployerId != employerId) return null;
            return MapToDto(job);
        }

        public async Task<JobDetailDto> CreateAsync(long employerId, CreateJobRequest request)
        {
            var employer = await _employerRepository.GetByIdAsync(employerId)
                ?? throw new Exception("Employer profile not found.");

            var job = new Job
            {
                EmployerId = employerId,
                Title = request.Title,
                Description = request.Description,
                Responsibilities = request.Responsibilities,
                Requirements = request.Requirements,
                Benefits = request.Benefits,
                Location = request.Location,
                WorkType = Enum.Parse<WorkType>(request.WorkType),
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                ExperienceLevel = Enum.Parse<ExperienceLevel>(request.ExperienceLevel),
                ApplicationDeadline = request.ApplicationDeadline,
                CategoryId = request.CategoryId,
                Status = JobStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            if (request.TechnologyIds != null && request.TechnologyIds.Any())
            {
                job.JobTechnologies = request.TechnologyIds
                    .Select(tid => new JobTechnology { TechnologyId = tid })
                    .ToList();
            }

            var created = await _jobRepository.CreateAsync(job);
            var result = await _jobRepository.GetByIdAsync(created.Id);
            return MapToDto(result!);
        }

        public async Task<JobDetailDto> UpdateAsync(long id, long employerId, UpdateJobRequest request)
        {
            var job = await _jobRepository.GetByIdAsync(id)
                ?? throw new Exception("Job not found.");
            if (job.EmployerId != employerId)
                throw new Exception("You don't have permission to update this job.");

            if (request.Title != null) job.Title = request.Title;
            if (request.Description != null) job.Description = request.Description;
            if (request.Responsibilities != null) job.Responsibilities = request.Responsibilities;
            if (request.Requirements != null) job.Requirements = request.Requirements;
            if (request.Benefits != null) job.Benefits = request.Benefits;
            if (request.Location != null) job.Location = request.Location;
            if (request.WorkType != null) job.WorkType = Enum.Parse<WorkType>(request.WorkType);
            if (request.SalaryMin.HasValue) job.SalaryMin = request.SalaryMin;
            if (request.SalaryMax.HasValue) job.SalaryMax = request.SalaryMax;
            if (request.ExperienceLevel != null) job.ExperienceLevel = Enum.Parse<ExperienceLevel>(request.ExperienceLevel);
            if (request.ApplicationDeadline.HasValue) job.ApplicationDeadline = request.ApplicationDeadline.Value;
            if (request.CategoryId.HasValue) job.CategoryId = request.CategoryId.Value;

            job.UpdatedAt = DateTime.UtcNow;
            _jobRepository.Update(job);

            var updated = await _jobRepository.GetByIdAsync(id);
            return MapToDto(updated!);
        }

        public async Task<bool> DeleteAsync(long id, long employerId)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null || job.EmployerId != employerId) return false;

            job.IsDeleted = true;
            job.DeletedAt = DateTime.UtcNow;
            _jobRepository.Update(job);
            return true;
        }

        public async Task<bool> CloseAsync(long id, long employerId)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null || job.EmployerId != employerId) return false;

            job.Status = JobStatus.Closed;
            job.UpdatedAt = DateTime.UtcNow;
            _jobRepository.Update(job);
            return true;
        }

        private static JobDetailDto MapToDto(Job j) => new()
        {
            Id = j.Id,
            EmployerId = j.EmployerId,
            CompanyName = j.Employer?.CompanyName ?? "",
            Title = j.Title,
            Description = j.Description,
            Responsibilities = j.Responsibilities,
            Requirements = j.Requirements,
            Benefits = j.Benefits,
            Location = j.Location,
            WorkType = j.WorkType.ToString(),
            SalaryMin = j.SalaryMin,
            SalaryMax = j.SalaryMax,
            ExperienceLevel = j.ExperienceLevel.ToString(),
            ApplicationDeadline = j.ApplicationDeadline,
            Status = j.Status.ToString(),
            RejectionReason = j.RejectionReason,
            ViewsCount = j.ViewsCount,
            ApplicationsCount = j.ApplicationsCount,
            CategoryName = j.Category?.Name ?? "",
            Technologies = j.JobTechnologies?.Select(jt => jt.Technology.Name).ToList() ?? new(),
            CreatedAt = j.CreatedAt,
            UpdatedAt = j.UpdatedAt
        };
    }
}

using Upwork.Application.DTOs.Employer;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;

namespace Upwork.Application.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
        }

        public async Task<EmployerProfileDto?> GetByIdAsync(long id)
        {
            var employer = await _employerRepository.GetByIdAsync(id);
            return employer == null ? null : MapToDto(employer);
        }

        public async Task<EmployerProfileDto?> GetByUserIdAsync(long userId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            return employer == null ? null : MapToDto(employer);
        }

        public async Task<List<EmployerProfileDto>> GetAllAsync()
        {
            var employers = await _employerRepository.GetAllAsync();
            return employers.Select(MapToDto).ToList();
        }

        public async Task<List<EmployerProfileDto>> SearchAsync(string? keyword)
        {
            var employers = await _employerRepository.SearchAsync(keyword);
            return employers.Select(MapToDto).ToList();
        }

        public async Task<EmployerProfileDto> CreateAsync(CreateEmployerProfileRequest request)
        {
            var exists = await _employerRepository.ExistsByUserIdAsync(request.UserId);
            if (exists)
                throw new Exception("User already has an employer profile.");

            var employer = new EmployerProfile
            {
                UserId = request.UserId,
                CompanyName = request.CompanyName,
                CompanyLogo = request.CompanyLogo,
                Description = request.Description,
                Industry = request.Industry,
                Website = request.Website,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _employerRepository.CreateAsync(employer);
            return MapToDto(created);
        }

        public async Task<EmployerProfileDto> UpdateAsync(long id, UpdateEmployerProfileRequest request)
        {
            var employer = await _employerRepository.GetByIdAsync(id)
                ?? throw new Exception("Employer profile not found.");

            if (request.CompanyName != null)
                employer.CompanyName = request.CompanyName;
            if (request.CompanyLogo != null)
                employer.CompanyLogo = request.CompanyLogo;
            if (request.Description != null)
                employer.Description = request.Description;
            if (request.Industry != null)
                employer.Industry = request.Industry;
            if (request.Website != null)
                employer.Website = request.Website;

            employer.UpdatedAt = DateTime.UtcNow;
            _employerRepository.Update(employer);

            var updated = await _employerRepository.GetByIdAsync(id);
            return MapToDto(updated!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var employer = await _employerRepository.GetByIdAsync(id);
            if (employer == null) return false;

            _employerRepository.Delete(employer);
            return true;
        }

        public async Task<List<EmployerJobDto>> GetJobsAsync(long employerId)
        {
            var employer = await _employerRepository.GetByIdAsync(employerId);
            if (employer == null) return new List<EmployerJobDto>();

            return employer.Jobs.Select(j => new EmployerJobDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                WorkType = j.WorkType.ToString(),
                SalaryMin = j.SalaryMin,
                SalaryMax = j.SalaryMax,
                ExperienceLevel = j.ExperienceLevel.ToString(),
                ApplicationDeadline = j.ApplicationDeadline,
                Status = j.Status.ToString(),
                ViewsCount = j.ViewsCount,
                ApplicationsCount = j.ApplicationsCount,
                CreatedAt = j.CreatedAt
            }).ToList();
        }

        private static EmployerProfileDto MapToDto(EmployerProfile e) => new()
        {
            Id = e.Id,
            UserId = e.UserId,
            UserName = e.User?.Name ?? "",
            UserEmail = e.User?.Email ?? "",
            CompanyName = e.CompanyName,
            CompanyLogo = e.CompanyLogo,
            Description = e.Description,
            Industry = e.Industry,
            Website = e.Website,
            JobsCount = e.Jobs?.Count ?? 0,
            ActiveSubscriptionsCount = e.Subscriptions?.Count(s => s.Status == Domain.Enums.SubscriptionStatus.Active) ?? 0,
            CreatedAt = e.CreatedAt
        };
    }
}

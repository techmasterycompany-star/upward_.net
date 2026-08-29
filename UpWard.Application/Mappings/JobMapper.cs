using Upwork.Application.DTOs.Common;
using Upwork.Domain.Entities;

namespace Upwork.Application.Mappings
{
    public static class JobMapper
    {
        public static JobDetailsDto ToDto(this Job job)
        {
            return new JobDetailsDto
            {
                Id = job.Id,
                EmployerId = job.EmployerId,
                CategoryId = job.CategoryId,
                CategoryName = job.Category.Name,
                Title = job.Title,
                Description = job.Description,
                Responsibilities = job.Responsibilities,
                Requirements = job.Requirements,
                Location = job.Location,
                WorkType = job.WorkType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                ExperienceLevel = job.ExperienceLevel,
                ApplicationDeadline = job.ApplicationDeadline,
                CreatedAt = job.CreatedAt,
                ViewsCount = job.ViewsCount,
                ApplicationsCount = job.ApplicationsCount
            };
        }
        public static JobSearchResultDto ToSearchResultDto(this Job job)
        {
            return new JobSearchResultDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                WorkType = job.WorkType,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                ExperienceLevel = job.ExperienceLevel,
                ApplicationDeadline = job.ApplicationDeadline,
                CreatedAt = job.CreatedAt,

                CategoryId = job.CategoryId,
                CategoryName = job.Category.Name,

                EmployerId = job.EmployerId,
                CompanyName = job.Employer.CompanyName
            };
        }

        public static SavedSearchDto ToDto(this SavedSearch search)
        {
            return new SavedSearchDto
            {
                Id = search.Id,
                Name = search.Name,
                Keyword = search.Keyword,
                Location = search.Location,
                CategoryId = search.CategoryId,
                WorkType = search.WorkType,
                MinSalary = search.MinSalary,
                MaxSalary = search.MaxSalary,
                ExperienceLevel = search.ExperienceLevel
            };
        }
    }
}

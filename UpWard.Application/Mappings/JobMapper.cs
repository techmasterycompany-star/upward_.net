using Upward.Application.DTOs.Common;
using Upward.Domain.Entities;

namespace Upward.Application.Mappings
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

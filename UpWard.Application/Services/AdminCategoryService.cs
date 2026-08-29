using Upwork.Application.DTOs.Admin;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;

namespace Upwork.Application.Services
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly ICategoryRepository repo;
        public AdminCategoryService(ICategoryRepository repo) => this.repo = repo;

        public async Task<AdminCategoryDto?> CreateAsync(CreateCategoryRequest request)
        {
            if (await NameExistsAsync(request.Name))
                return null;

            var category = new Category
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim()
            };
            await repo.AddAsync(category);
            return new AdminCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                JobsCount = 0
            };
        }
        public async Task<bool> NameExistsAsync(string name, long? excludeId = null)
        {
            var categories = await repo.GetAllAsync();

            return categories.Any(c =>
                c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var category = await repo.GetByIdAsync(id);
            if (category is null) return false;
            await repo.DeleteAsync(category);
            return true;
        }

        public async Task<List<AdminCategoryDto>> GetAllAsync()
        {
            var categories = await repo.GetAllAsync();
            return categories.Select(c => new AdminCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                JobsCount = c.Jobs.Count
            }).ToList();
        }

        public async Task<AdminCategoryDto?> GetByIdAsync(long id)
        {
            var category = await repo.GetByIdAsync(id);
            if (category is null) return null;

            return new AdminCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                JobsCount = category.Jobs.Count
            };
        }

        public async Task<AdminCategoryDto?> UpdateAsync(long id, UpdateCategoryRequest request)
        {
            var category = await repo.GetByIdAsync(id);
            if (category is null) return null;

            category.Name = request.Name.Trim();
            category.Description = request.Description?.Trim();

            await repo.UpdateAsync(category);

            return new AdminCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                JobsCount = category.Jobs.Count
            };
        }
    }
}

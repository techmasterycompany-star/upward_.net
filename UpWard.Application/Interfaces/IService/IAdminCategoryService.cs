using Upwork.Application.DTOs.Admin;

namespace Upwork.Application.Interfaces.IService
{
    public interface IAdminCategoryService
    {
        Task<List<AdminCategoryDto>> GetAllAsync();
        Task<AdminCategoryDto?> GetByIdAsync(long id);
        Task<bool> NameExistsAsync(string name, long? excludeId = null);
        Task<AdminCategoryDto> CreateAsync(CreateCategoryRequest request);
        Task<AdminCategoryDto?> UpdateAsync(long id, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(long id);
    }
}

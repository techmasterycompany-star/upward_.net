using Upward.Application.DTOs.Admin;

namespace Upward.Application.Interfaces.IService
{
    public interface IAdminTechnologyService
    {
        Task<List<AdminTechnologyDto>> GetTechnologiesAsync();
        Task<AdminTechnologyDto?> GetByIdAsync(long id);
        Task<bool> NameExistsAsync(string name, long? excludeId = null);
        Task<AdminTechnologyDto?> CreateTechnologyAsync(CreateTechnologyRequest request);
        Task<AdminTechnologyDto?> UpdateTechnologyAsync(long id, UpdateTechnologyRequest request);
        Task<bool> DeleteTechnologyAsync(long id);
    }
}

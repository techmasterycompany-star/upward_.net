using Upwork.Application.DTOs.Admin;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;

namespace Upwork.Application.Services
{
    public class AdminTechnologyService : IAdminTechnologyService
    {
        private readonly ITechnologyRepository repo;
        public AdminTechnologyService(ITechnologyRepository repo) => this.repo = repo;


        public async Task<AdminTechnologyDto> CreateTechnologyAsync(CreateTechnologyRequest request)
        {
            if (await NameExistsAsync(request.Name))
                return null;

            var tech = new Technology { Name = request.Name.Trim() };
            await repo.AddAsync(tech);
            return new AdminTechnologyDto
            {
                Id = tech.Id,
                Name = tech.Name,
            };
        }

        public async Task<bool> DeleteTechnologyAsync(long id)
        {
            var tech = await repo.GetByIdAsync(id);
            if (tech is null) return false;
            await repo.DeleteAsync(tech);
            return true;
        }

        public async Task<AdminTechnologyDto?> GetByIdAsync(long id)
        {
            var technology = await repo.GetByIdAsync(id);
            if (technology is null) return null;

            return new AdminTechnologyDto { Id = technology.Id, Name = technology.Name };
        }

        public async Task<List<AdminTechnologyDto>> GetTechnologiesAsync()
        {
            var techs = await repo.GetAllAsync();
            return techs.Select(t => new AdminTechnologyDto
            {
                Id = t.Id,
                Name = t.Name,
            }).ToList();
        }

        public async Task<bool> NameExistsAsync(string name, long? excludeId = null)
        {
            var technologies = await repo.GetAllAsync();
            return technologies.Any(t =>
                t.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) &&
                (!excludeId.HasValue || t.Id != excludeId.Value));
        }

        public async Task<AdminTechnologyDto?> UpdateTechnologyAsync(long id, UpdateTechnologyRequest request)
        {
            var tech = await repo.GetByIdAsync(id);
            if (tech is null) return null;
            tech.Name = request.Name.Trim();
            await repo.UpdateAsync(tech);
            return new AdminTechnologyDto
            {
                Id = tech.Id,
                Name = tech.Name,
            };
        }
    }
}

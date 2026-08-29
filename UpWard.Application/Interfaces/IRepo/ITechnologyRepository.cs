using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface ITechnologyRepository
    {
        Task<List<Technology>> GetAllAsync();
        Task<Technology?> GetByIdAsync(long id);
        Task AddAsync(Technology technology);
        Task UpdateAsync(Technology technology);
        Task DeleteAsync(Technology technology);
    }

}

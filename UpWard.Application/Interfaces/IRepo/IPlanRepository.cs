using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllAsync();
        Task<Plan?> GetByIdAsync(long id);
    }
}

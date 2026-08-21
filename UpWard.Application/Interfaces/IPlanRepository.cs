using Upward.Domain.Entities;

namespace Upward.Application.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllAsync();
        Task<Plan?> GetByIdAsync(long id);
    }
}

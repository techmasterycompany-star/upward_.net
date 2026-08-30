using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IEmployerRepository
    {
        Task<EmployerProfile?> GetByIdAsync(long id);
        Task<EmployerProfile?> GetByUserIdAsync(long userId);
        Task<List<EmployerProfile>> GetAllAsync();
        Task<List<EmployerProfile>> SearchAsync(string? keyword);
        Task<EmployerProfile> CreateAsync(EmployerProfile employer);
        void Update(EmployerProfile employer);
        void Delete(EmployerProfile employer);
        Task<bool> ExistsByUserIdAsync(long userId);
        Task<Subscription?> GetSubscriptionByUserId(long id);
    }
}

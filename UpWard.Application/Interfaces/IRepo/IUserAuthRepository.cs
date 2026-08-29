using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface IUserAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(long id);
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}

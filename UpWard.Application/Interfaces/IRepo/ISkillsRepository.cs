using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface ISkillsRepository
    {
        Task<Skill?> GetByNameAsync(string name);
        Task<Skill?> GetByIdAsync(long id);
        Task AddAsync(Skill skill);
        Task SaveChangesAsync();
    }
}

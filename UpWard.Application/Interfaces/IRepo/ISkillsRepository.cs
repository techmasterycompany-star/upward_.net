using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces.IRepo
{
    public interface ISkillsRepository
    {
        Task<Skill?> GetByNameAsync(string name);

        Task AddAsync(Skill skill);

        Task SaveChangesAsync();
    }
}

using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface ISkillsRepository
    {
        Task<Skill?> GetByNameAsync(string name);

        Task AddAsync(Skill skill);

        Task SaveChangesAsync();
    }
}

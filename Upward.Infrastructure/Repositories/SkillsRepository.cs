using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class SkillsRepository : ISkillsRepository
    {
        private readonly AppDBContext _context;

        public SkillsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Skill?> GetByNameAsync(string name)
        {
            var normalized = name.Trim();

            return await _context.Skills
                .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower());
        }

        public async Task AddAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

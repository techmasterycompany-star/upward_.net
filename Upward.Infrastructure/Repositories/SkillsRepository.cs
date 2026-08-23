using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
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

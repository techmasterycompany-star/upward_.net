using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class CandidateProfileRepository : ICandidateProfileRepository
    {
        private readonly AppDBContext _context;

        public CandidateProfileRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<CandidateProfile?> GetByUserIdAsync(long userId)
        {
            return await _context.CandidateProfiles
                .Include(x => x.User)
                .Include(x => x.CandidateSkills)
                .ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task<bool> ExistsAsync(long userId)
        {
            return _context.CandidateProfiles.AnyAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(CandidateProfile profile)
        {
            await _context.CandidateProfiles.AddAsync(profile);
        }

        public void Update(CandidateProfile profile)
        {
            _context.CandidateProfiles.Update(profile);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

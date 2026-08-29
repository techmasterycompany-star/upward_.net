using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private readonly AppDBContext context;
        public TechnologyRepository(AppDBContext context) => this.context = context;


        public async Task AddAsync(Technology technology)
        {
            await context.Technologies.AddAsync(technology);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Technology technology)
        {
            context.Technologies.Remove(technology);
            await context.SaveChangesAsync();
        }

        public async Task<List<Technology>> GetAllAsync() => 
            await context.Technologies
                .Include(t => t.JobTechnologies)
                .OrderBy(t => t.Name)
                .ToListAsync();

        public async Task<Technology?> GetByIdAsync(long id) =>
            await context.Technologies
                 .Include(t => t.JobTechnologies)
                 .FirstOrDefaultAsync(t => t.Id == id);
        
        public async Task UpdateAsync(Technology technology)
        {
            context.Technologies.Update(technology);
            await context.SaveChangesAsync();
        }
    }
}

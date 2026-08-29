using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly AppDBContext _context;

        public PlanRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(long id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}

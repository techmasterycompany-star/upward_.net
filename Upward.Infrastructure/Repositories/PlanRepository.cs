using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
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

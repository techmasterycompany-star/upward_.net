using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly AppDBContext context;
        public EmployerRepository(AppDBContext context) => this.context = context;

        public async Task<EmployerProfile?> GetByIdAsync(long id) =>
            await context.EmployerProfiles
            .Include(e => e.User)
            .Include(e => e.Jobs)
            .Include(e => e.Subscriptions)
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<EmployerProfile?> GetByUserIdAsync(long userId) =>
            await context.EmployerProfiles
            .Include(e => e.User)
            .Include(e => e.Jobs)
            .Include(e => e.Subscriptions)
            .FirstOrDefaultAsync(e => e.UserId == userId);

        public async Task<List<EmployerProfile>> GetAllAsync() =>
            await context.EmployerProfiles
            .Include(e => e.User)
            .Include(e => e.Jobs)
            .Include(e => e.Subscriptions)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        public async Task<List<EmployerProfile>> SearchAsync(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();

            var cleanKeyword = keyword.Trim();
            return await context.EmployerProfiles
            .Include(e => e.User)
            .Include(e => e.Jobs)
            .Include(e => e.Subscriptions)
            .Where(e => e.CompanyName.Contains(cleanKeyword))
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        }

        public async Task<EmployerProfile> CreateAsync(EmployerProfile employer)
        {
            await context.EmployerProfiles.AddAsync(employer);
            await context.SaveChangesAsync();
            return employer;
        }

        public void Update(EmployerProfile employer)
        {
            context.EmployerProfiles.Update(employer);
        }

        public void Delete(EmployerProfile employer)
        {
            context.EmployerProfiles.Remove(employer);
        }

        public async Task<bool> ExistsByUserIdAsync(long userId) =>
            await context.EmployerProfiles.AnyAsync(e => e.UserId == userId);
    }
}

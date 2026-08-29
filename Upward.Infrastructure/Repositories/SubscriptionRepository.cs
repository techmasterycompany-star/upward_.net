using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDBContext _context;

        public SubscriptionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Subscription?> GetByStripeSessionIdAsync(string sessionId)
        {
            return await _context.Subscriptions
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.StripeSessionId == sessionId);
        }

        public async Task AddAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await SaveChanges();
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await SaveChanges();
        }
        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}

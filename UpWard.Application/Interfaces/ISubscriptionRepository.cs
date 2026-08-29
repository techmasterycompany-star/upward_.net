using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetByStripeSessionIdAsync(string sessionId);
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
    }
}

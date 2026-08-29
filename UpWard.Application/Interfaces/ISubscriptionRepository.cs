using Upward.Domain.Entities;

namespace Upward.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetByStripeSessionIdAsync(string sessionId);
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
    }
}

using Upwork.Application.DTOs;

namespace Upwork.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<PlanDto>> GetAvailablePlansAsync();
        Task<CheckoutResponseDto> CreateCheckoutAsync(long employerId, CreateCheckoutRequest request);
        Task HandleStripeWebhookAsync(string payload, string stripeSignature);
    }
}

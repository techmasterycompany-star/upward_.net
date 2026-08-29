using Upward.Application.DTOs;

namespace Upward.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<PlanDto>> GetAvailablePlansAsync();
        Task<CheckoutResponseDto> CreateCheckoutAsync(long employerId, CreateCheckoutRequest request);
        Task HandleStripeWebhookAsync(string payload, string stripeSignature);
    }
}

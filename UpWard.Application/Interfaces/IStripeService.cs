using Upward.Application.DTOs;

namespace Upward.Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(StripeCheckoutRequest request);
        Stripe.Event ConstructWebhookEvent(string payload, string stripeSignature);
    }
}

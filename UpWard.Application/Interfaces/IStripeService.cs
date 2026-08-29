using Upwork.Application.DTOs;

namespace Upwork.Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(StripeCheckoutRequest request);
        Stripe.Event ConstructWebhookEvent(string payload, string stripeSignature);
    }
}

using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Upward.Application.DTOs;
using Upward.Application.Interfaces;
using Upward.Domain.Enums;

namespace Upward.Infrastructure.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly string _webhookSecret;

        public StripeService(IConfiguration configuration)
        {
            var secretKey = configuration["Stripe:SecretKey"]
                ?? throw new InvalidOperationException("Stripe:SecretKey is not configured.");

            _webhookSecret = configuration["Stripe:WebhookSecret"]
                ?? throw new InvalidOperationException("Stripe:WebhookSecret is not configured.");

            StripeConfiguration.ApiKey = secretKey;
        }

        public async Task<string> CreateCheckoutSessionAsync(StripeCheckoutRequest request)
        {
            var amountInCents = (long)(request.Amount * 100);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency    = request.Currency,
                            UnitAmount  = amountInCents,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"UpWard — {request.PlanName} Plan " +
                                       $"({(request.BillingCycle == BillingCycle.Yearly ? "Yearly" : "Monthly")})"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode       = "payment",
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
                Metadata   = new Dictionary<string, string>
                {
                    { "employer_id", request.EmployerId.ToString() }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return $"{session.Id}|{session.Url}";
        }

        public Event ConstructWebhookEvent(string payload, string stripeSignature)
        {
            return EventUtility.ConstructEvent(
                payload,
                stripeSignature,
                _webhookSecret,
                throwOnApiVersionMismatch: false);
        }
    }
}

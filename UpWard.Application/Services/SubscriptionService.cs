using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Upward.Application.DTOs;
using Upward.Application.Interfaces;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IPlanRepository _planRepo;
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IEmployerRepository _employerRepo;
        private readonly IStripeService _stripeService;
        private readonly IConfiguration _configuration;

        public SubscriptionService(
            IPlanRepository planRepo,
            ISubscriptionRepository subscriptionRepo,
            IPaymentRepository paymentRepo,
            IEmployerRepository employerRepo,
            IStripeService stripeService,
            IConfiguration configuration)
        {
            _planRepo = planRepo;
            _subscriptionRepo = subscriptionRepo;
            _paymentRepo = paymentRepo;
            _employerRepo = employerRepo;
            _stripeService = stripeService;
            _configuration = configuration;
        }


        public async Task<IEnumerable<PlanDto>> GetAvailablePlansAsync()
        {
            var plans = await _planRepo.GetAllAsync();

            return plans.Select(p => new PlanDto
            {
                Id = p.Id,
                Name = p.Name,
                JobPostLimit = p.JobPostLimit,
                PriceMonthly = p.PriceMonthly,
                PriceYearly = p.PriceYearly,
                IsFeatured = p.IsFeatured,
                HasDirectMessaging = p.HasDirectMessaging,
                HasPremiumReports = p.HasPremiumReports
            });
        }

        public async Task<CheckoutResponseDto> CreateCheckoutAsync(long employerId, CreateCheckoutRequest request)
        {
            var emp = await _employerRepo.GetByIdAsync(employerId) 
                ?? throw new InvalidOperationException($"Employer with ID {employerId} was not found.");

            var plan = await _planRepo.GetByIdAsync(request.PlanId)
                ?? throw new InvalidOperationException($"Plan with ID {request.PlanId} was not found.");

            var amount = request.BillingCycle == BillingCycle.Yearly
                ? plan.PriceYearly
                : plan.PriceMonthly;

            if (amount <= 0)
                throw new InvalidOperationException("Cannot checkout a free plan. No payment is required.");

            var successUrl = _configuration["Stripe:SuccessUrl"];
            var cancelUrl  = _configuration["Stripe:CancelUrl"];

            var stripeRequest = new StripeCheckoutRequest
            {
                EmployerId   = employerId,
                PlanName     = plan.Name,
                Amount       = amount,
                Currency     = "usd",
                BillingCycle = request.BillingCycle,
                SuccessUrl   = successUrl,
                CancelUrl    = cancelUrl
            };

            var checkoutUrl = await _stripeService.CreateCheckoutSessionAsync(stripeRequest);

            var parts     = checkoutUrl.Split('|', 2);
            var sessionId = parts[0];
            var url       = parts[1];

            var now = DateTime.UtcNow;


            var subscription = new Upward.Domain.Entities.Subscription
            {
                EmployerId          = employerId,
                PlanId              = plan.Id,
                BillingCycle        = request.BillingCycle,
                Status              = SubscriptionStatus.Pending,
                StripeSessionId     = sessionId,
                CurrentPeriodStart  = now,
                CurrentPeriodEnd    = request.BillingCycle == BillingCycle.Yearly
                                        ? now.AddYears(1)
                                        : now.AddMonths(1),
                CreatedAt           = now,
                UpdatedAt           = now
            };

            await _subscriptionRepo.AddAsync(subscription);

            var payment = new Upward.Domain.Entities.Payment
            {
                SubscriptionId        = subscription.Id,
                Amount                = amount,
                Currency              = "usd",
                Gateway               = PaymentGateway.Stripe,
                GatewayTransactionId  = null,   
                Status                = PaymentStatus.Pending,
                PaidAt                = null,
                CreatedAt             = now,
                UpdatedAt             = now
            };

            await _paymentRepo.AddAsync(payment);

            return new CheckoutResponseDto { CheckoutUrl = url };
        }

        public async Task HandleStripeWebhookAsync(string payload, string stripeSignature)
        {
            var stripeEvent = _stripeService.ConstructWebhookEvent(payload, stripeSignature);

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null) return;

                var sessionId = session.Id;

                var subscription = await _subscriptionRepo.GetByStripeSessionIdAsync(sessionId);
                if (subscription == null)
                {
                    return;
                }

                var now = DateTime.UtcNow;

                subscription.Status    = SubscriptionStatus.Active;
                subscription.UpdatedAt = now;

                await _subscriptionRepo.UpdateAsync(subscription);

                var pendingPayment = subscription.Payments.FirstOrDefault(p => p.Status == PaymentStatus.Pending);
                if (pendingPayment != null)
                {
                    pendingPayment.Status               = PaymentStatus.Completed;
                    pendingPayment.GatewayTransactionId = session.PaymentIntentId ?? session.Id;
                    pendingPayment.PaidAt               = now;
                    pendingPayment.UpdatedAt            = now;

                    await _paymentRepo.UpdateAsync(pendingPayment);
                }
            }
        }
    }
}

using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Upwork.Application.DTOs;
using Upwork.Application.Interfaces;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Exceptions;
using Upwork.Domain.Enums;

namespace Upwork.Application.Services
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
            var emp = await _employerRepo.GetByUserIdAsync(employerId) 
                ?? throw new NotFoundException($"Employer with ID {employerId} was not found.");

            var plan = await _planRepo.GetByIdAsync(request.PlanId)
                ?? throw new NotFoundException($"Plan with ID {request.PlanId} was not found.");

            var amount = request.BillingCycle == BillingCycle.Yearly
                ? plan.PriceYearly
                : plan.PriceMonthly;

            if (amount <= 0)
                throw new InvalidOperationException("Cannot checkout a free plan. No payment is required.");

            var userSubscription = await _employerRepo.GetSubscriptionByUserId(emp.Id);
            if(userSubscription != null && userSubscription.Status == SubscriptionStatus.Active)
                throw new InvalidOperationException("Employer already has an active subscription.");

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


            var subscription = new Upwork.Domain.Entities.Subscription
            {
                EmployerId          = emp.Id,
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

            var payment = new Upwork.Domain.Entities.Payment
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


                if (session == null) throw new InvalidOperationException("Session is null.");

                if(session.PaymentStatus != "paid") throw new InvalidOperationException("Payment status is not paid.");

                var sessionId = session.Id;

                var subscription = await _subscriptionRepo.GetByStripeSessionIdAsync(sessionId);
                if (subscription == null) throw new NotFoundException("Subscription not found.");

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

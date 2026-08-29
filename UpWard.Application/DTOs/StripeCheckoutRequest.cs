using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs
{
    public class StripeCheckoutRequest
    {
        public long EmployerId { get; set; }
        public string PlanName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public BillingCycle BillingCycle { get; set; }
        public string SuccessUrl { get; set; } = null!;
        public string CancelUrl { get; set; } = null!;
    }
}

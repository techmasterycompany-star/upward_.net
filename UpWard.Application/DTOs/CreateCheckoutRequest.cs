using Upward.Domain.Enums;

namespace Upward.Application.DTOs
{
    public class CreateCheckoutRequest
    {
        public long PlanId { get; set; }
        public BillingCycle BillingCycle { get; set; }
    }
}

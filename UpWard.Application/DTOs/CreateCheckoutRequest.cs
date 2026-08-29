using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs
{
    public class CreateCheckoutRequest
    {
        public long PlanId { get; set; }
        public BillingCycle BillingCycle { get; set; }
    }
}

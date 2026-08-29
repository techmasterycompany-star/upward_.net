using Upward.Domain.Common;
using Upward.Domain.Enums;

namespace Upward.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public long EmployerId { get; set; }

        public long PlanId { get; set; }

        public BillingCycle BillingCycle { get; set; }

        public SubscriptionStatus Status { get; set; }

        public DateTime CurrentPeriodStart { get; set; }

        public DateTime CurrentPeriodEnd { get; set; }

        public EmployerProfile Employer { get; set; } = null!;

        public Plan Plan { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}

using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;

        public int? JobPostLimit { get; set; }

        public decimal PriceMonthly { get; set; }

        public decimal PriceYearly { get; set; }

        public bool IsFeatured { get; set; }

        public bool HasDirectMessaging { get; set; }

        public bool HasPremiumReports { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}

namespace Upwork.Application.DTOs
{
    public class PlanDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int? JobPostLimit { get; set; }
        public decimal PriceMonthly { get; set; }
        public decimal PriceYearly { get; set; }
        public bool IsFeatured { get; set; }
        public bool HasDirectMessaging { get; set; }
        public bool HasPremiumReports { get; set; }
    }
}

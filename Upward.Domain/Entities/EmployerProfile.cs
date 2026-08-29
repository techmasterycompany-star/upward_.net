using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class EmployerProfile : BaseEntity
    {
        public long UserId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string? CompanyLogo { get; set; }

        public string? Description { get; set; }

        public string? Industry { get; set; }

        public string? Website { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}

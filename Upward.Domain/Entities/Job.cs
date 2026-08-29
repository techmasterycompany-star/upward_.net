using Upwork.Domain.Common;
using Upwork.Domain.Enums;

namespace Upwork.Domain.Entities
{
    public class Job : SoftDeletableEntity
    {
        public long EmployerId { get; set; }

        public long CategoryId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Responsibilities { get; set; } = null!;

        public string Requirements { get; set; } = null!;

        public string? Location { get; set; }

        public WorkType WorkType { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public ExperienceLevel ExperienceLevel { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public JobStatus Status { get; set; }

        public string? RejectionReason { get; set; }

        public int ViewsCount { get; set; }

        public int ApplicationsCount { get; set; }

        // Navigation properties
        public EmployerProfile Employer { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public ICollection<JobTechnology> JobTechnologies { get; set; } = new List<JobTechnology>();

        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public ICollection<JobView> JobViews { get; set; } = new List<JobView>();
    }
}

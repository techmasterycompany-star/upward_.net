using Upwork.Domain.Common;
using Upwork.Domain.Enums;

namespace Upwork.Domain.Entities
{
    public class User : SoftDeletableEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }
        public bool IsSuspended { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        // Navigation properties
        public EmployerProfile? EmployerProfile { get; set; }

        public CandidateProfile? CandidateProfile { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<CommentReport> CommentReports { get; set; } = new List<CommentReport>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

namespace Upward.Application.DTOs.Employer
{
    public class EmployerProfileDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string? CompanyLogo { get; set; }
        public string? Description { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
        public int JobsCount { get; set; }
        public int ActiveSubscriptionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

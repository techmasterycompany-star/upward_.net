namespace Upward.Application.DTOs.Employer
{
    public class JobAnalyticsDto
    {
        public long JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public int ViewsCount { get; set; }
        public int ApplicationsCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class EmployerDashboardDto
    {
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int TotalViews { get; set; }
        public List<JobAnalyticsDto> TopJobs { get; set; } = new();
    }

    public class CandidateSearchDto
    {
        public long CandidateId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Headline { get; set; }
        public string? Location { get; set; }
        public string? Resume { get; set; }
        public List<string> Skills { get; set; } = new();
        public bool IsDiscoverable { get; set; }
    }
}

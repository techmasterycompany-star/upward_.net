namespace Upward.Application.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int Candidates { get; set; }
        public int Employers { get; set; }
        public int Admins { get; set; }
        public int TotalJobs { get; set; }
        public int PendingJobs { get; set; }
        public int ApprovedJobs { get; set; }
        public int RejectedJobs { get; set; }
        public int TotalApplications { get; set; }
        public int TotalComments { get; set; }
        public int HiddenComments { get; set; }
    }
}

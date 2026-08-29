using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository repo;
        public AdminDashboardService(IAdminDashboardRepository repo) => this.repo = repo;

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var userCounts = await repo.CountUsersByRoleAsync();
            var jobCounts = await repo.GetJobCountsByStatusAsync();

            userCounts.TryGetValue(UserRole.Candidate, out var candidates);
            userCounts.TryGetValue(UserRole.Employer, out var employers);
            userCounts.TryGetValue(UserRole.Admin, out var admins);

            jobCounts.TryGetValue(JobStatus.PendingApproval, out var pendingJobs);
            jobCounts.TryGetValue(JobStatus.Approved, out var approvedJobs);
            jobCounts.TryGetValue(JobStatus.Rejected, out var rejectedJobs);

            return new AdminDashboardDto
            {
                TotalUsers = userCounts.Values.Sum(),
                Candidates = candidates,
                Employers = employers,
                Admins = admins,
                TotalJobs = jobCounts.Values.Sum(),
                PendingJobs = pendingJobs,
                ApprovedJobs = approvedJobs,
                RejectedJobs = rejectedJobs,
                TotalApplications = await repo.CountApplicationsAsync(),
                TotalComments = await repo.CountCommentsAsync(),
                HiddenComments = await repo.CountHiddenCommentsAsync()
            };
        }
    }
}

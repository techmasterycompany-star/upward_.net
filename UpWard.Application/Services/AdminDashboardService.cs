using Upwork.Application.DTOs.Admin;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Enums;

namespace Upwork.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository repo;
        public AdminDashboardService(IAdminDashboardRepository repo) => this.repo = repo;

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var jobCounts = await repo.GetJobCountsByStatusAsync();

            jobCounts.TryGetValue(JobStatus.PendingApproval, out var pendingJobs);
            jobCounts.TryGetValue(JobStatus.Approved, out var approvedJobs);
            jobCounts.TryGetValue(JobStatus.Rejected, out var rejectedJobs);
            return new AdminDashboardDto
            {
                TotalUsers = await repo.CountUsersAsync(),
                Candidates = await repo.CountUsersByRoleAsync(UserRole.Candidate),
                Employers = await repo.CountUsersByRoleAsync(UserRole.Employer),
                Admins = await repo.CountUsersByRoleAsync(UserRole.Admin),
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

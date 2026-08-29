using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Enums;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly AppDBContext context;
        public AdminDashboardRepository(AppDBContext context) => this.context = context;



        public async Task<int> CountApplicationsAsync() =>
             await context.Applications.CountAsync(a => !a.IsDeleted);


        public async Task<int> CountCommentsAsync() =>
            await context.Comments.CountAsync(a => !a.IsDeleted);


        public async Task<int> CountHiddenCommentsAsync() =>
            await context.Comments.CountAsync(a => !a.IsDeleted && !a.IsApproved);


        public async Task<Dictionary<UserRole , int>> CountUsersByRoleAsync() =>
            await context.Users
                .Where(u => !u.IsDeleted)
                .GroupBy(u => u.Role)
                .ToDictionaryAsync(u => u.Key, u => u.Count());

        public async Task<Dictionary<JobStatus, int>> GetJobCountsByStatusAsync() =>
            await context.Jobs
                 .Where(j => !j.IsDeleted)
                 .GroupBy(j => j.Status)
                 .ToDictionaryAsync(x => x.Key, x => x.Count());
        
    }
}

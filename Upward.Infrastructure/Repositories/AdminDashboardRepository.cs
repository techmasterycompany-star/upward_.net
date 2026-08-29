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



        public Task<int> CountApplicationsAsync() =>
             context.Applications.CountAsync(a => !a.IsDeleted);


        public async Task<int> CountCommentsAsync() =>
            await context.Comments.CountAsync(a => !a.IsDeleted);


        public async Task<int> CountHiddenCommentsAsync() =>
            await context.Comments.CountAsync(a => !a.IsDeleted && !a.IsApproved);


        public async Task<int> CountUsersAsync() =>
            await context.Users.CountAsync(u =>  !u.IsDeleted);
       

        public async Task<int> CountUsersByRoleAsync(UserRole role) =>
            await context.Users.CountAsync(u => !u.IsDeleted &&  u.Role == role);

        public async Task<Dictionary<JobStatus, int>> GetJobCountsByStatusAsync() =>
            await context.Jobs
                 .Where(j => !j.IsDeleted)
                 .GroupBy(j => j.Status)
                 .Select(g => new { g.Key, Count = g.Count() })
                 .ToDictionaryAsync(x => x.Key, x => x.Count);
        
    }
}

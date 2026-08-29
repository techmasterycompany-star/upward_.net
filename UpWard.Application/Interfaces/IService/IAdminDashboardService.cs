using Upwork.Application.DTOs.Admin;

namespace Upwork.Application.Interfaces.IService
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

    }
}

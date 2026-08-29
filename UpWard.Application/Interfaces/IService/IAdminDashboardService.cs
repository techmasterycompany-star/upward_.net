using Upward.Application.DTOs.Admin;

namespace Upward.Application.Interfaces.IService
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

    }
}

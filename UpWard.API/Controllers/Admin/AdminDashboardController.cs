using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers.Admin
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService service;
        public AdminDashboardController(IAdminDashboardService service) => this.service = service;



        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await service.GetDashboardAsync();
            return Ok(dashboard);
        }
    }
}

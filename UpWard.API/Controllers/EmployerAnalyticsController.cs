using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Employer;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
{
    [Route("api/employer/analytics")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerAnalyticsController : ControllerBase
    {
        private readonly IEmployerAnalyticsService _service;
        public EmployerAnalyticsController(IEmployerAnalyticsService service) => _service = service;

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var dashboard = await _service.GetDashboardAsync(userId);
            return Ok(dashboard);
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> GetJobAnalytics()
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var analytics = await _service.GetJobAnalyticsAsync(userId);
            return Ok(analytics);
        }

        [HttpGet("candidates")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchCandidates([FromQuery] string? keyword, [FromQuery] List<string>? skills)
        {
            var candidates = await _service.SearchCandidatesAsync(keyword, skills);
            return Ok(candidates);
        }
    }
}

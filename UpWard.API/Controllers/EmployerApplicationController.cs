using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.DTOs.Employer;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers
{
    [Route("api/employer/applications")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerApplicationController : ControllerBase
    {
        private readonly IEmployerApplicationService _service;
        public EmployerApplicationController(IEmployerApplicationService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var applications = await _service.GetApplicationsByEmployerAsync(userId);
            return Ok(applications);
        }

        [HttpGet("job/{jobId:long}")]
        public async Task<IActionResult> GetByJob(long jobId)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var applications = await _service.GetApplicationsByJobAsync(jobId, userId);
            return Ok(applications);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var application = await _service.GetByIdAsync(id, userId);
            if (application == null) return NotFound("Application not found.");
            return Ok(application);
        }

        [HttpPatch("{id:long}/accept")]
        public async Task<IActionResult> Accept(long id)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var result = await _service.AcceptAsync(id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:long}/reject")]
        public async Task<IActionResult> Reject(long id, [FromBody] ReviewApplicationRequest request)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var result = await _service.RejectAsync(id, userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

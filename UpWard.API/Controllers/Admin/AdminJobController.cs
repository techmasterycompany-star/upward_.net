using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers.Admin
{
    [Route("api/admin/jobs")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminJobController : ControllerBase
    {
        private readonly IAdminJobService service;
        public AdminJobController(IAdminJobService service) => this.service = service;


        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await service.GetJobsAsync();
            return Ok(jobs);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingJobs()
        {
            var jobs = await service.GetPendingJobsAsync();
            return Ok(jobs);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetJob(long id)
        {
            var job = await service.GetJobAsync(id);
            if (job is null)
                return NotFound($"Job with id {id} was not found.");

            return Ok(job);
        }
        [HttpPost("{id:long}/approve")]
        public async Task<IActionResult> ApproveJob(long id)
        {
            try
            {
                await service.ApproveJobAsync(id);
                return Ok(new { message = "Job has been approved successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id:long}/reject")]
        public async Task<IActionResult> RejectJob(long id, [FromBody] RejectJobRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Reason))
                return BadRequest("Rejection reason is required.");

            try
            {
                await service.RejectJobAsync(id, request.Reason);
                return Ok(new { message = "Job has been rejected successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.DTOs.Employer;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers
{
    [Route("api/employer/jobs")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerJobController : ControllerBase
    {
        private readonly IEmployerJobService _service;
        public EmployerJobController(IEmployerJobService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var jobs = await _service.GetJobsByEmployerAsync(userId);
            return Ok(jobs);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var job = await _service.GetByIdAsync(id, userId);
            if (job == null) return NotFound("Job not found.");
            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobRequest request)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var job = await _service.CreateAsync(userId, request);
                return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateJobRequest request)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var job = await _service.UpdateAsync(id, userId, request);
                return Ok(job);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _service.DeleteAsync(id, userId);
            if (!result) return NotFound("Job not found.");
            return NoContent();
        }

        [HttpPatch("{id:long}/close")]
        public async Task<IActionResult> Close(long id)
        {
            var userId = long.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _service.CloseAsync(id, userId);
            if (!result) return NotFound("Job not found.");
            return Ok(new { message = "Job closed successfully." });
        }
    }
}

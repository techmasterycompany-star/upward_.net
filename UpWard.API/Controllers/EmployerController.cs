using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Employer;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
{
    [Route("api/employer/profile")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _service;
        public EmployerController(IEmployerService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var profile = await _service.GetByUserIdAsync(userId);
            if (profile == null)
                return NotFound("Employer profile not found.");
            return Ok(profile);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            var profile = await _service.GetByIdAsync(id);
            if (profile == null)
                return NotFound($"Employer profile with id {id} was not found.");
            return Ok(profile);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var profiles = await _service.SearchAsync(keyword);
            return Ok(profiles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateEmployerProfileRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            request.UserId = userId;
            try
            {
                var profile = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProfile(long id, [FromBody] UpdateEmployerProfileRequest request)
        {
            try
            {
                var profile = await _service.UpdateAsync(id, request);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProfile(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound("Employer profile not found.");
            return NoContent();
        }

        [HttpGet("{id:long}/jobs")]
        public async Task<IActionResult> GetJobs(long id)
        {
            var jobs = await _service.GetJobsAsync(id);
            return Ok(jobs);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upwork.API.Helpers;
using Upwork.Application.DTOs.Candidate;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers
{
    [Route("api/candidate/applications")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidateApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public CandidateApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost("jobs/{jobId:long}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Apply(long jobId, [FromForm] ApplyJobRequestDto request)
        {

            try
            {
                var userId = ClaimsHelper.GetUserId(User);
                var application = await _applicationService.ApplyAsync(userId, jobId, request);

                return StatusCode(StatusCodes.Status201Created, application);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("jobs/{jobId:long}/using-profile")]
        public async Task<IActionResult> ApplyUsingProfile(long jobId, ApplyUsingProfileDto applyDto)
        {

            try
            {
                var userId = ClaimsHelper.GetUserId(User);
                var application = await _applicationService.ApplyUsingProfileAsync(userId, jobId, applyDto);

                return StatusCode(StatusCodes.Status201Created, application);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyApplications()
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var applications = await _applicationService.GetMyApplicationsAsync(userId);

                return Ok(applications);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
        [HttpGet("{applicationId:long}")]
        public async Task<IActionResult> GetApplicationById(long applicationId)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var application = await _applicationService.GetByIdAsync(userId,applicationId);

                return application is null? NotFound("Job application not found.") : Ok(application);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{applicationId:long}/cancel")]
        public async Task<IActionResult> Cancel(long applicationId)
        {

            try
            {
                var userId = ClaimsHelper.GetUserId(User);
                await _applicationService.CancelAsync(userId, applicationId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upward.API.Helpers;
using Upward.Application.DTOs.Candidate;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
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

                return Ok(application);
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

                return Ok(application);
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

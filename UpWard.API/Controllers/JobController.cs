using Microsoft.AspNetCore.Mvc;
using Upward.API.Helpers;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchJobs([FromQuery] JobSearchRequestDto request)
        {
            try
            {
                var result = await _jobService.SearchAsync(request);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{jobId:long}")]
        public async Task<IActionResult> GetJob(long jobId)
        {
            try
            {
                var result = await _jobService.GetByIdAsync(jobId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{jobId:long}/view")]
        public async Task<IActionResult> RecordView(long jobId)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _jobService.RecordViewAsync(jobId, userId, ipAddress);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("saved-searches")]
        public async Task<IActionResult> SaveSearch([FromBody] SaveJobSearchRequestDto request)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var result = await _jobService.SaveSearchAsync(userId, request);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("saved-searches")]
        public async Task<IActionResult> GetSavedSearches()
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var result = await _jobService.GetSavedSearchesAsync(userId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("saved-searches/{savedSearchId:long}")]
        public async Task<IActionResult> DeleteSavedSearch(long savedSearchId)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                await _jobService.DeleteSavedSearchAsync(userId, savedSearchId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Upward.API.Helpers;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IService;
using Upward.Application.Services;

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
            var result = await _jobService.SearchAsync(request);

            return Ok(result);
        }

        [HttpGet("{jobId:long}")]
        public async Task<IActionResult> GetJob(long jobId)
        {
            var result = await _jobService.GetByIdAsync(jobId);

            return Ok(result);
        }

        [HttpPost("{jobId:long}/view")]
        public async Task<IActionResult> RecordView(long jobId)
        {
            var userId = ClaimsHelper.GetUserId(User);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _jobService.RecordViewAsync(jobId, userId, ipAddress);

            return NoContent();
        }

        [HttpPost("saved-searches")]
        public async Task<IActionResult> SaveSearch([FromBody] SaveJobSearchRequestDto request)
        {
            var userId = ClaimsHelper.GetUserId(User);

            var result = await _jobService.SaveSearchAsync(userId, request);

            return Ok(result);
        }

        [HttpGet("saved-searches")]
        public async Task<IActionResult> GetSavedSearches()
        {
            var userId = ClaimsHelper.GetUserId(User);

            var result = await _jobService.GetSavedSearchesAsync(userId);

            return Ok(result);
        }

        [HttpDelete("saved-searches/{savedSearchId:long}")]
        public async Task<IActionResult> DeleteSavedSearch(long savedSearchId)
        {
            var userId = ClaimsHelper.GetUserId(User);

            await _jobService.DeleteSavedSearchAsync(userId, savedSearchId);

            return NoContent();
        }
    }

}

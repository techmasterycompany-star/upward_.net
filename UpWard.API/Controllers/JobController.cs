using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            //TODO: Save Search Keyword

            return Ok(result);
        }
    }

}

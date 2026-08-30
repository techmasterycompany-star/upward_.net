using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.DTOs;
using Upwork.Application.Interfaces;

namespace Upwork.API.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CreateCheckout(
            [FromQuery] long employerId,
            [FromBody]  CreateCheckoutRequest request)
        {
            if (employerId <= 0)
                return BadRequest(new { message = "A valid employerId query parameter is required." });

            try
            {
                var result = await _subscriptionService.CreateCheckoutAsync(employerId, request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

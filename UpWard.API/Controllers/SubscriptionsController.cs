using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.DTOs;
using Upwork.Application.Interfaces;
using Upwork.Application.DTOs;
using Upwork.Application.Interfaces;
using Upwork.API.Helpers;
using Upwork.Application.Exceptions;

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
        public async Task<IActionResult> CreateCheckout([FromBody]  CreateCheckoutRequest request)
        {
            try
            {
                var employerId = ClaimsHelper.GetUserId(User);
                var result = await _subscriptionService.CreateCheckoutAsync(employerId, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}

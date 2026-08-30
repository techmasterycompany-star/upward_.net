using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs;
using Upward.Application.Interfaces;
using Upward.API.Helpers;
using Upward.Application.Exceptions;

namespace Upward.API.Controllers
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

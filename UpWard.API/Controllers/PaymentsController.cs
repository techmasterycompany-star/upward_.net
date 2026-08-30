using Microsoft.AspNetCore.Mvc;
using Upward.Application.Interfaces;
using Upward.Application.Exceptions;

namespace Upward.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public PaymentsController(
            ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("stripe/webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            string payload;
            using (var reader = new StreamReader(Request.Body))
            {
                payload = await reader.ReadToEndAsync();
            }

            var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(stripeSignature))
            {
                return BadRequest(new { message = "Missing Stripe-Signature header." });
            }

            try
            {
                await _subscriptionService.HandleStripeWebhookAsync(payload, stripeSignature);
                return Ok();
            }
            catch (Stripe.StripeException ex)
            {
                return BadRequest(new { message = "Invalid webhook signature." });
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
                return StatusCode(500, new { message = "An error occurred processing the webhook." });
            }
        }
    }
}

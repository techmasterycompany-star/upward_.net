using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized(new { Message = "User identity could not be determined from the token." });

            try
            {
                var result = await _notificationService.GetAllAsync(userId.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving notifications.", Detail = ex.Message });
            }
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized(new { Message = "User identity could not be determined from the token." });

            try
            {
                var result = await _notificationService.GetUnreadAsync(userId.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving unread notifications.", Detail = ex.Message });
            }
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized(new { Message = "User identity could not be determined from the token." });

            try
            {
                var count = await _notificationService.GetUnreadCountAsync(userId.Value);
                return Ok(new { UnreadCount = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the unread notification count.", Detail = ex.Message });
            }
        }

        [HttpPatch("{id:long}/read")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized(new { Message = "User identity could not be determined from the token." });

            try
            {
                var success = await _notificationService.MarkAsReadAsync(userId.Value, id);

                return success
                    ? NoContent()
                    : NotFound(new { Message = $"Notification with ID {id} was not found or does not belong to the current user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while marking notification {id} as read.", Detail = ex.Message });
            }
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized(new { Message = "User identity could not be determined from the token." });

            try
            {
                await _notificationService.MarkAllAsReadAsync(userId.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while marking all notifications as read.", Detail = ex.Message });
            }
        }

        private long? GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");

            return long.TryParse(claim, out var id) ? id : null;
        }
    }
}

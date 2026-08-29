using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Upward.Application.Interfaces.IService;
using Upward.API.Helpers;

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
            var userId = ClaimsHelper.GetUserId(User);

            try
            {
                var result = await _notificationService.GetAllAsync(userId);
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
            var userId = ClaimsHelper.GetUserId(User);

            try
            {
                var result = await _notificationService.GetUnreadAsync(userId);
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
            var userId = ClaimsHelper.GetUserId(User);

            try
            {
                var count = await _notificationService.GetUnreadCountAsync(userId);
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
            var userId = ClaimsHelper.GetUserId(User);

            try
            {
                var success = await _notificationService.MarkAsReadAsync(userId, id);

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
            var userId = ClaimsHelper.GetUserId(User);

            try
            {
                await _notificationService.MarkAllAsReadAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while marking all notifications as read.", Detail = ex.Message });
            }
        }

    }
}

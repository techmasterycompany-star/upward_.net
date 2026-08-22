using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers.Admin
{
    [Route("api/admin/comments")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminCommentController : ControllerBase
    {
        private readonly IAdminCommentService service;
        public AdminCommentController(IAdminCommentService service) => this.service = service;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await service.GetCommentsAsync();
            return Ok(comments);
        }
        [HttpPatch("{id:long}/hide")]
        public async Task<IActionResult> Hide(long id)
        {
            var success = await service.HideCommentAsync(id);
            if (!success) return NotFound($"Comment with id {id} was not found or is already hidden.");

            return Ok(new { message = "Comment has been hidden successfully." });
        }

        [HttpPatch("{id:long}/restore")]
        public async Task<IActionResult> Restore(long id)
        {
            var success = await service.RestoreCommentAsync(id);
            if (!success) return NotFound($"Comment with id {id} was not found or is already active.");

            return Ok(new { message = "Comment has been restored successfully." });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await service.DeleteCommentAsync(id);
            if (!success) return NotFound($"Comment with id {id} was not found.");

            return NoContent();
        }
    }
}

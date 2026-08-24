using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.API.Helpers;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers
{
    [Route("api/job/{jobId:long}/comments")]
    [ApiController]
    [Authorize(Roles = "Candidate,Employer")]
    public class JobCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public JobCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(long jobId)
        {
            try
            {
                var comments = await _commentService.GetByJobIdAsync(jobId);

                return Ok(comments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(long jobId, [FromBody] CreateCommentDto request)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var comment = await _commentService.CreateAsync(userId, jobId, request);

                return Ok(comment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{commentId:long}")]
        public async Task<IActionResult> UpdateComment(long commentId, [FromBody] UpdateCommentDto request)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var comment = await _commentService.UpdateAsync(userId, commentId, request);

                return comment is null ? NotFound(new { message = "Comment not found." }) : Ok(comment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{commentId:long}")]
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);

                var deleted = await _commentService.DeleteAsync(userId, commentId);

                return deleted ? NoContent() : NotFound(new { message = "Comment not found." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Enums;

namespace Upward.API.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService service;
        public AdminUserController(IAdminUserService service) => this.service = service;

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filter)
        {
            var users = await service.GetUsersAsync(filter);
            return Ok(users);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await service.GetUserAsync(id);
            if (user is null) 
                return NotFound($"User with id {id} was not found.");

            return Ok(user);
        }

        [HttpPatch("{id:long}/suspend")]
        public async Task<IActionResult> SuspendUser(long id)
        {
            var result = await service.SuspendUserAsync(id);
            if (!result) 
                return BadRequest("Could not suspend user. Check if user exists, is an Admin, or is already suspended.");

            return Ok(new { message = "User has been suspended successfully." });
        }

        [HttpPatch("{id:long}/activate")]
        public async Task<IActionResult> ActivateUser(long id)
        {
            var result = await service.ActivateUserAsync(id);
            if (!result) 
                return BadRequest("Could not activate user. Check if user exists or is already active.");

            return Ok(new { message = "User has been activated successfully." });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await service.DeleteUserAsync(id);
            if (!result) 
                return BadRequest("Could not delete user. Check if user exists or is an Admin.");

            return Ok(new { message = "User has been deleted successfully." });
        }

    }
}

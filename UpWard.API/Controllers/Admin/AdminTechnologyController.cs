using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upwork.Application.DTOs.Admin;
using Upwork.Application.Interfaces.IService;

namespace Upwork.API.Controllers.Admin
{
    [Route("api/admin/technologies")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminTechnologyController : ControllerBase
    {
        private readonly IAdminTechnologyService service;
        public AdminTechnologyController(IAdminTechnologyService service) => this.service = service;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var technologies = await service.GetTechnologiesAsync();
            return Ok(technologies);
        }
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var technology = await service.GetByIdAsync(id);
            return technology is null ? NotFound($"Technology with id {id} was not found.") : Ok(technology);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTechnologyRequest request)
        {
            if (await service.NameExistsAsync(request.Name))
                return Conflict($"A technology with the name '{request.Name.Trim()}' already exists.");

            var technology = await service.CreateTechnologyAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = technology?.Id }, technology);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id,[FromBody] UpdateTechnologyRequest request)
        {
            if (await service.NameExistsAsync(request.Name , id))
                return Conflict($"A technology with the name '{request.Name.Trim()}' already exists.");

            var technology = await service.UpdateTechnologyAsync(id, request);
            if (technology is null) return NotFound($"Technology with id {id} was not found.");

            return Ok(technology);
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await service.DeleteTechnologyAsync(id);
            if (!success) return BadRequest("Could not delete technology. Check if technology exists or is associated with jobs.");

            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IService;

namespace Upward.API.Controllers.Admin
{
    [Route("api/admin/categories")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IAdminCategoryService service;
        public AdminCategoryController(IAdminCategoryService service) => this.service = service;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var category = await service.GetByIdAsync(id);
            if (category is null) return NotFound($"Category with id {id} was not found.");
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            if (await service.NameExistsAsync(request.Name))
                return Conflict($"A category with the name '{request.Name.Trim()}' already exists.");

            var category = await service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = category?.Id }, category);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateCategoryRequest request)
        {
            if (await service.NameExistsAsync(request.Name, id))
                return Conflict($"A category with the name '{request.Name.Trim()}' already exists.");

            var category = await service.UpdateAsync(id, request);
            if (category is null) return NotFound($"Category with id {id} was not found.");

            return Ok(category);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await service.DeleteAsync(id);
            if (!result) return NotFound($"Category with id {id} was not found or could not be deleted.");

            return NoContent();
        }
    }
}

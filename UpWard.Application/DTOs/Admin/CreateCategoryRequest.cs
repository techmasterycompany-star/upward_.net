using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Admin
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}

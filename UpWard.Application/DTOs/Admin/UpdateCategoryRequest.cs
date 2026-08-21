using System.ComponentModel.DataAnnotations;

namespace Upward.Application.DTOs.Admin
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}

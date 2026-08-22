using System.ComponentModel.DataAnnotations;

namespace Upward.Application.DTOs.Admin
{
    public class UpdateTechnologyRequest
    {
        [Required(ErrorMessage = "Technology name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}

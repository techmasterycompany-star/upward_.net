
using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Admin
{
    public class CreateTechnologyRequest
    {
        [Required(ErrorMessage = "Technology name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Common
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 2000 characters.")]
        public string Content { get; set; } = null!;
    }


}

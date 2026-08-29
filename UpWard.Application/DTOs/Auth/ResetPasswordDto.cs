using System.ComponentModel.DataAnnotations;

namespace Upward.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Reset token is required.")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Compare("NewPassword", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

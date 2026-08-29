using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Auth
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage = "Verification token is required.")]
        public string Token { get; set; } = null!;
    }
}

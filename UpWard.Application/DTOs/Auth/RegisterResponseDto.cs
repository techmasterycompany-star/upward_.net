namespace Upward.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public long UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}

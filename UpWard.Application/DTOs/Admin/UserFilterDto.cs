using Upward.Domain.Enums;

namespace Upward.Application.DTOs.Admin
{
    public class UserFilterDto
    {
        public string? Search { get; set; }
        public UserRole? Role { get; set; }
        public bool? IsSuspended { get; set; }
    }
}

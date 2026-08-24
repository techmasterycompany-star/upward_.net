namespace Upward.Application.DTOs.Employer
{
    public class CreateEmployerProfileRequest
    {
        public long UserId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? CompanyLogo { get; set; }
        public string? Description { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
    }
}

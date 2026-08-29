namespace Upwork.Application.DTOs.Employer
{
    public class UpdateEmployerProfileRequest
    {
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public string? Description { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
    }
}

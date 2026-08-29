using Microsoft.AspNetCore.Http;

namespace Upwork.Application.DTOs.Candidate
{
    public class ApplyJobRequestDto
    {
        public IFormFile Resume { get; set; } = null!;

        public string? CoverLetter { get; set; }

        public string? Message { get; set; }

        public string ContactEmail { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public bool ConfirmContactInformation { get; set; }
    }

    public class ApplyUsingProfileDto
    {

        public string? CoverLetter { get; set; }

        public string? Message { get; set; }

        public string ContactPhone { get; set; } = null!;

    }


}

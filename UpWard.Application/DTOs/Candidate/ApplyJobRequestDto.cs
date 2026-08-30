using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Candidate
{
    public class ApplyJobRequestDto
    {
        [Required]
        public IFormFile Resume { get; set; } = null!;

        [StringLength(5000)]
        public string? CoverLetter { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string ContactEmail { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(30)]
        public string ContactPhone { get; set; } = null!;

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm your contact information.")]
        public bool ConfirmContactInformation { get; set; }
    }

    public class ApplyUsingProfileDto
    {
        [StringLength(5000)]
        public string? CoverLetter { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        [Required]
        [Phone]
        [StringLength(30)]
        public string ContactPhone { get; set; } = null!;
    }


}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Upwork.Application.DTOs.Candidate
{
    public class UploadResumeRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}

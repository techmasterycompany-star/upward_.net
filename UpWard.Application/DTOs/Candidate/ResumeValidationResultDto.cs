using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upward.Application.DTOs.Candidate
{
    public class ResumeValidationResultDto
    {
        public bool IsValid { get; set; }
        public string? ErrorMesssage { get; set; }
    }

    public class UploadResumeRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}

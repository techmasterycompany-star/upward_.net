using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upward.Application.DTOs.Candidate
{
    public class FileUploadResult
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
    }
}

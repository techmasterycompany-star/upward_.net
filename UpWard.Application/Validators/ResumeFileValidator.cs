using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Application.DTOs.Candidate;

namespace Upward.Application.Validators
{
    public static class ResumeFileValidator
    {
        private const long DefaultMaxFileSize = 5 * 1024 * 1024;

        private const string PdfContentType = "application/pdf";

        public static async Task<ResumeValidationResultDto> ValidateAsync(ResumeFileDto file, long? maxFileSize = null)
        {
            var effectiveMaxFileSize = maxFileSize ?? DefaultMaxFileSize;

            if (file is null)
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = "Resume file is required."
                };
            }

            if (file.Length <= 0)
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = "Resume file cannot be empty."
                };
            }

            if (file.Length > effectiveMaxFileSize)
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = $"Resume file cannot exceed {effectiveMaxFileSize / 1024 / 1024} MB."
                };
            }

            var extension = Path.GetExtension(file.FileName);

            if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = "Only PDF files are allowed."
                };
            }

            if (!string.Equals( file.ContentType, PdfContentType, StringComparison.OrdinalIgnoreCase))
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = "Invalid resume file type. Only PDF files are allowed."
                };
            }

            if (!await IsPdfSignatureAsync(file.Content))
            {
                return new ResumeValidationResultDto
                {
                    IsValid = false,
                    ErrorMesssage = "The uploaded file is not a valid PDF."
                };
            }

            return new ResumeValidationResultDto { IsValid = true };
        }

        private static async Task<bool> IsPdfSignatureAsync(Stream stream)
        {
            if (!stream.CanSeek)
                return false;

            stream.Position = 0;

            var buffer = new byte[4];

            var bytesRead = await stream.ReadAsync(buffer);

            stream.Position = 0;

            return bytesRead == 4 &&
                   buffer[0] == 0x25 && // %
                   buffer[1] == 0x50 && // P
                   buffer[2] == 0x44 && // D
                   buffer[3] == 0x46;   // F
        }
    }
}

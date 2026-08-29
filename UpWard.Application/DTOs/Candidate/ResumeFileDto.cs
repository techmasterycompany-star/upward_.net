using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upward.Application.DTOs.Candidate
{
    public class ResumeFileDto
    {
        public Stream Content { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long Length { get; set; }
    }
}

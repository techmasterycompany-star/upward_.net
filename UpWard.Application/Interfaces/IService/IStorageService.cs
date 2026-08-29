using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upwork.Application.DTOs.Candidate;

namespace Upwork.Application.Interfaces.IService
{
    public interface IStorageService
    {
        Task<FileUploadResult> UploadAsync(Stream file,string fileName,string contentType, string folder = "resumes");

        Task DeleteAsync(string publicId);

        Task<string> GetDownloadUrlAsync(string publicId);

    }
}

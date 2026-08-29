using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Upward.Application.DTOs.Candidate;
using Upward.Application.Interfaces.IService;

namespace Upward.Infrastructure.FileStorage
{
    public class CloudinaryStorageService : IStorageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryStorageService(
            IOptions<CloudinarySettings> settings)
        {
            var account = new Account(
                settings.Value.CloudName,
                settings.Value.ApiKey,
                settings.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }
        public async Task<FileUploadResult> UploadAsync(Stream file, string fileName, string contentType, string folder = "resumes")
        {

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, file),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                throw new InvalidOperationException(
                    $"Cloudinary upload failed: {result.Error.Message}");
            }

            return new FileUploadResult
            {
                PublicId = result.PublicId,
                Url = result.Url?.ToString() ?? string.Empty,
                SecureUrl = result.SecureUrl?.ToString() ?? string.Empty,
                Format = result.Format ?? string.Empty
            };
        }

        public async Task DeleteAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                throw new InvalidOperationException(
                    $"Cloudinary deletion failed: {result.Error.Message}");
            }
        }


        public Task<string> GetDownloadUrlAsync(string publicId)
        {
            var url = _cloudinary.Api.Url
                .ResourceType("raw")
                .Secure(true)
                .BuildUrl(publicId);
            return Task.FromResult(url);
        }
    }
}

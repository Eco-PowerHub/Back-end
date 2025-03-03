using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcoPowerHub.Models;
using Microsoft.Extensions.Options;

namespace EcoPowerHub.Repositories.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<(string imageUrl, string publicId)> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = Path.GetFileNameWithoutExtension(fileName), // حفظ public_id
                Transformation = new Transformation().Quality(80).FetchFormat("jpg")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return (uploadResult.SecureUrl?.AbsoluteUri, uploadResult.PublicId);
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}
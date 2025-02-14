using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using EcoPowerHub.Repositories.Interfaces;
using System.Linq;
using AngleSharp.Io.Dom;

namespace EcoPowerHub.Repositories.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Transformation = new Transformation().Quality(80).FetchFormat("jpg") // Optimize image
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.AbsoluteUri;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileStream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.AbsoluteUri;
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}

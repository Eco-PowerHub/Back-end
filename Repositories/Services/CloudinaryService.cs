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
                PublicId = Path.GetFileNameWithoutExtension(fileName), // حفظ `public_id`
                Transformation = new Transformation().Quality(80).FetchFormat("jpg")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
<<<<<<< HEAD
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileStream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.AbsoluteUri;
=======
            return (uploadResult.SecureUrl?.AbsoluteUri, uploadResult.PublicId);
>>>>>>> 7740503d34511a176c63549010bb78c4a74d82cf
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}

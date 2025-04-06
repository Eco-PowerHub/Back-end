namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string publicId);
    }
}

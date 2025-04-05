using AngleSharp.Media.Dom;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.Repositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(ICloudinaryService cloudinaryService, ILogger<FileUploadController> logger)
        {
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
           _logger.LogInformation($"Uploading file: {file.FileName}, Size: {file.Length} bytes");
            var imageUrl = await _cloudinaryService.UploadImageAsync(file);
            _logger.LogInformation($"File uploaded successfully: {imageUrl}");

            return Ok(new { imageUrl });  
        }

        [HttpDelete("delete/{publicId}")]
        public async Task<IActionResult> DeleteFile(string publicId)
        {
            var result = await _cloudinaryService.DeleteFileAsync(publicId);
            return result ? Ok("Deleted successfully") : BadRequest("Failed to delete");
        }
    }
}

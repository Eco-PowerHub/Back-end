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
            //if (file == null || file.Length == 0)
            //    return BadRequest("No file uploaded");

            //using var stream = file.OpenReadStream();
            //var (url, publicId) = await _cloudinaryService.UploadImageAsync(stream, file.FileName);

            //return Ok(new { ImageUrl = url, PublicId = publicId });
            try
            {
                _logger.LogInformation($"Uploading file: {file.FileName}, Size: {file.Length} bytes");
                var imageUrl = await _cloudinaryService.UploadImageAsync(file);

                // Log successful upload
                _logger.LogInformation($"File uploaded successfully: {imageUrl}");
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred during file upload");
                return StatusCode(500, new
                {
                    message = "An error occurred while uploading the file",
                    details = ex.Message
                });
            }
        }

        [HttpDelete("delete/{publicId}")]
        public async Task<IActionResult> DeleteFile(string publicId)
        {
            var result = await _cloudinaryService.DeleteFileAsync(publicId);
            return result ? Ok("Deleted successfully") : BadRequest("Failed to delete");
        }
    }
}

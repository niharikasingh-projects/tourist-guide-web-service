using Microsoft.AspNetCore.Mvc;

namespace TouristGuide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController: ControllerBase
    {
        [HttpGet("profiles/{fileName}")]
        public async Task<IActionResult> GetProfileImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest(new { message = "File name is required." });

            var imagePath = Path.Combine(Environment.CurrentDirectory, "images", "profiles", fileName);

            if (!System.IO.File.Exists(imagePath))
                return NotFound(new { message = "Image not found." });

            var contentType = GetContentType(imagePath);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

            return File(fileBytes, contentType);
        }

        [HttpGet("attractions/{fileName}")]
        public async Task<IActionResult> GetAttractionImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest(new { message = "File name is required." });

            var imagePath = Path.Combine(Environment.CurrentDirectory, "images", "attractions", fileName);

            if (!System.IO.File.Exists(imagePath))
                return NotFound(new { message = "Image not found." });

            var contentType = GetContentType(imagePath);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

            return File(fileBytes, contentType);
        }

        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}

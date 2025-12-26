using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null) return null;

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Location = user.Location,
                Languages = user.Languages,
                Certifications = user.Certifications,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserProfileDto?> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null) return null;

            // Update only provided fields
            if (dto.Name != null) user.Name = dto.Name;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            if (dto.DateOfBirth.HasValue) user.DateOfBirth = dto.DateOfBirth;
            if (dto.Location != null) user.Location = dto.Location;
            if (dto.Languages != null) user.Languages = dto.Languages;
            if (dto.Certifications != null) user.Certifications = dto.Certifications;
            if (dto.ProfileImageUrl != null) user.ProfileImageUrl = await ProcessAttractionPictureAsync(dto.ProfileImageUrl);

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetUserProfileAsync(userId);
        }

        private async Task<string> ProcessAttractionPictureAsync(IFormFile picture)
        {
            string profileImageUrl = string.Empty;

            // Allowed file types (MIME types)
            var allowedTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif" };
            // Max file size (e.g., 2 MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (picture != null && picture.Length > 0)
            {
                try
                {
                    if (!allowedTypes.Contains(picture.ContentType))
                    {
                        throw new Exception("Invalid file type. Only JPG, PNG, and GIF are allowed.");
                    }

                    // Validate file size
                    if (picture.Length > maxFileSize)
                    {
                        throw new Exception("File size exceeds 2 MB limit.");
                    }

                    var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "images", "profiles");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }

                    profileImageUrl = $"/images/profiles/{fileName}";
                }
                catch (IOException)
                {
                    // Log the exception as needed
                    //return StatusCode(500, new { message = "An error occurred while saving the file.", detail = ioEx.Message });
                    throw;
                }
                catch (Exception)
                {
                    // Log the exception as needed
                    //return StatusCode(500, new { message = "Unexpected error during file upload.", detail = ex.Message });
                    throw;
                }
            }

            return profileImageUrl;
        }
    }
}

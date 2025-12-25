using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public class AttractionService : IAttractionService
    {
        private readonly ApplicationDbContext _context;

        public AttractionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttractionDto>> GetAllAttractionsAsync()
        {
            return await _context.TouristAttractions
                .Where(a => a.IsActive)
                .Select(a => new AttractionDto
                {
                    Id = a.Id,
                    AttractionName = a.Name,
                    Description = a.Description,
                    Location = a.Location,
                    ImageUrl = a.ImageUrl,
                    Category = a.Category,
                    Rating = a.Rating,
                    EntryFee = a.EntryFee,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        public async Task<AttractionDto?> GetAttractionByIdAsync(int id)
        {
            var attraction = await _context.TouristAttractions.FindAsync(id);
            if (attraction == null) return null;

            return new AttractionDto
            {
                Id = attraction.Id,
                AttractionName = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                ImageUrl = attraction.ImageUrl,
                Category = attraction.Category,
                Rating = attraction.Rating,
                EntryFee = attraction.EntryFee,
                IsActive = attraction.IsActive
            };
        }

        public async Task<AttractionDto> CreateAttractionAsync(CreateAttractionDto dto)
        {
            var imageURL = await ProcessAttractionPictureAsync(dto?.AttractionPicture);

            var attraction = new TouristAttraction
            {
                Name = dto.AttractionName,
                Description = dto.Description,
                Location = dto.Location,
                ImageUrl = imageURL,
                Category = dto.Category,
                Rating = dto.Rating,
                EntryFee = dto.EntryFee,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.TouristAttractions.Add(attraction);
            await _context.SaveChangesAsync();

            return new AttractionDto
            {
                Id = attraction.Id,
                AttractionName = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                ImageUrl = imageURL,
                Category = attraction.Category,
                Rating = attraction.Rating,
                EntryFee = attraction.EntryFee,
                IsActive = attraction.IsActive
            };
        }

        public async Task<AttractionDto?> UpdateAttractionAsync(int id, UpdateAttractionDto dto)
        {
            var attraction = await _context.TouristAttractions.FindAsync(id);
            if (attraction == null) return null;

            var imageURL = await ProcessAttractionPictureAsync(dto?.AttractionPicture);

            if (dto?.AttractionName != null) attraction.Name = dto.AttractionName;
            if (dto?.Description != null) attraction.Description = dto.Description;
            if (dto?.Location != null) attraction.Location = dto.Location;
            if (dto?.AttractionPicture != null) attraction.ImageUrl = imageURL;
            if (dto?.Category != null) attraction.Category = dto.Category;
            if (dto.Rating.HasValue) attraction.Rating = dto.Rating.Value;
            if (dto.EntryFee.HasValue) attraction.EntryFee = dto.EntryFee.Value;
            if (dto.IsActive.HasValue) attraction.IsActive = dto.IsActive.Value;

            attraction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new AttractionDto
            {
                Id = attraction.Id,
                AttractionName = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                ImageUrl = imageURL,
                Category = attraction.Category,
                Rating = attraction.Rating,
                EntryFee = attraction.EntryFee,
                IsActive = attraction.IsActive
            };
        }

        public async Task<bool> DeleteAttractionAsync(int id)
        {
            var attraction = await _context.TouristAttractions.FindAsync(id);
            if (attraction == null) return false;

            attraction.IsActive = false;
            attraction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AttractionDto>> SearchAttractionsAsync(string location)
        {
            return await _context.TouristAttractions
                .Where(a => a.IsActive && a.Location.Contains(location))
                .Select(a => new AttractionDto
                {
                    Id = a.Id,
                    AttractionName = a.Name,
                    Description = a.Description,
                    Location = a.Location,
                    ImageUrl = a.ImageUrl,
                    Category = a.Category,
                    Rating = a.Rating,
                    EntryFee = a.EntryFee,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AttractionDto>> GetAttractionsByLocationAsync(string location)
        {

            return await _context.TouristAttractions
                .Where(a => a.IsActive && (location.Contains(a.Location.ToLower()) || a.Location.ToLower() == location.ToLower()))
                .Select(a => new AttractionDto
                {
                    Id = a.Id,
                    AttractionName = a.Name,
                    Description = a.Description,
                    Location = a.Location,
                    ImageUrl = a.ImageUrl,
                    Category = a.Category,
                    Rating = a.Rating,
                    EntryFee = a.EntryFee,
                    IsActive = a.IsActive
                })
                .ToListAsync();
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

                    var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "images", "attractions");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }

                    profileImageUrl = $"/images/attractions/{fileName}";
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

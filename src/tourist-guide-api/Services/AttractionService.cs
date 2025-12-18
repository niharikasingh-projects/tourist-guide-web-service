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
                    Name = a.Name,
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
                Name = attraction.Name,
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
            var attraction = new TouristAttraction
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                ImageUrl = dto.ImageUrl,
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
                Name = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                ImageUrl = attraction.ImageUrl,
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

            if (dto.Name != null) attraction.Name = dto.Name;
            if (dto.Description != null) attraction.Description = dto.Description;
            if (dto.Location != null) attraction.Location = dto.Location;
            if (dto.ImageUrl != null) attraction.ImageUrl = dto.ImageUrl;
            if (dto.Category != null) attraction.Category = dto.Category;
            if (dto.Rating.HasValue) attraction.Rating = dto.Rating.Value;
            if (dto.EntryFee.HasValue) attraction.EntryFee = dto.EntryFee.Value;
            if (dto.IsActive.HasValue) attraction.IsActive = dto.IsActive.Value;

            attraction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new AttractionDto
            {
                Id = attraction.Id,
                Name = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                ImageUrl = attraction.ImageUrl,
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
                    Name = a.Name,
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
    }
}

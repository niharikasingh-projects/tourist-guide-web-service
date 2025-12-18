using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public class GuideService : IGuideService
    {
        private readonly ApplicationDbContext _context;

        public GuideService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuideProfileDto>> GetGuidesByAttractionIdAsync(int attractionId, string? timeFrom = null, string? timeTo = null)
        {
            var query = _context.GuideProfiles
                .Include(g => g.Attraction)
                .Where(g => g.AttractionId == attractionId && g.IsAvailable);

            var guides = await query.ToListAsync();

            // If time filtering is required, check availability
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                var availableGuides = new List<GuideProfile>();
                foreach (var guide in guides)
                {
                    if (await IsGuideAvailableAsync(guide.Id, DateTime.Today, timeFrom, timeTo))
                    {
                        availableGuides.Add(guide);
                    }
                }
                guides = availableGuides;
            }

            return guides.Select(g => new GuideProfileDto
            {
                Id = g.Id,
                UserId = g.UserId,
                AttractionId = g.AttractionId,
                AttractionName = g.Attraction.Name,
                FullName = g.FullName,
                Email = g.Email,
                PhoneNumber = g.PhoneNumber,
                Experience = g.Experience,
                Languages = g.Languages,
                Bio = g.Bio,
                Rating = g.Rating,
                PricePerHour = g.PricePerHour,
                Availability = g.Availability,
                ProfileImageUrl = g.ProfileImageUrl,
                IsAvailable = g.IsAvailable
            });
        }

        public async Task<GuideProfileDto?> GetGuideProfileByIdAsync(int id)
        {
            var guide = await _context.GuideProfiles
                .Include(g => g.Attraction)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guide == null) return null;

            return new GuideProfileDto
            {
                Id = guide.Id,
                UserId = guide.UserId,
                AttractionId = guide.AttractionId,
                AttractionName = guide.Attraction.Name,
                FullName = guide.FullName,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber,
                Experience = guide.Experience,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                Availability = guide.Availability,
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable
            };
        }

        public async Task<GuideProfileDto?> GetGuideProfileByUserIdAsync(int userId)
        {
            var guide = await _context.GuideProfiles
                .Include(g => g.Attraction)
                .FirstOrDefaultAsync(g => g.UserId == userId);

            if (guide == null) return null;

            return new GuideProfileDto
            {
                Id = guide.Id,
                UserId = guide.UserId,
                AttractionId = guide.AttractionId,
                AttractionName = guide.Attraction.Name,
                FullName = guide.FullName,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber,
                Experience = guide.Experience,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                Availability = guide.Availability,
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable
            };
        }

        public async Task<GuideProfileDto> CreateGuideProfileAsync(int userId, CreateGuideProfileDto dto)
        {
            // Check if guide profile already exists for this user
            if (await _context.GuideProfiles.AnyAsync(g => g.UserId == userId && g.AttractionId == dto.AttractionId))
            {
                throw new Exception("Guide profile already exists for this attraction");
            }

            var guide = new GuideProfile
            {
                UserId = userId,
                AttractionId = dto.AttractionId,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Experience = dto.Experience,
                Languages = dto.Languages,
                Bio = dto.Bio,
                Rating = dto.Rating,
                PricePerHour = dto.PricePerHour,
                Availability = dto.Availability,
                ProfileImageUrl = dto.ProfileImageUrl,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.GuideProfiles.Add(guide);
            await _context.SaveChangesAsync();

            var attraction = await _context.TouristAttractions.FindAsync(dto.AttractionId);

            return new GuideProfileDto
            {
                Id = guide.Id,
                UserId = guide.UserId,
                AttractionId = guide.AttractionId,
                AttractionName = attraction?.Name ?? "",
                FullName = guide.FullName,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber,
                Experience = guide.Experience,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                Availability = guide.Availability,
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable
            };
        }

        public async Task<GuideProfileDto?> UpdateGuideProfileAsync(int userId, UpdateGuideProfileDto dto)
        {
            var guide = await _context.GuideProfiles
                .Include(g => g.Attraction)
                .FirstOrDefaultAsync(g => g.UserId == userId);

            if (guide == null) return null;

            if (dto.FullName != null) guide.FullName = dto.FullName;
            if (dto.PhoneNumber != null) guide.PhoneNumber = dto.PhoneNumber;
            if (dto.Experience.HasValue) guide.Experience = dto.Experience.Value;
            if (dto.Languages != null) guide.Languages = dto.Languages;
            if (dto.Bio != null) guide.Bio = dto.Bio;
            if (dto.Rating.HasValue) guide.Rating = dto.Rating.Value;
            if (dto.PricePerHour.HasValue) guide.PricePerHour = dto.PricePerHour.Value;
            if (dto.Availability != null) guide.Availability = dto.Availability;
            if (dto.ProfileImageUrl != null) guide.ProfileImageUrl = dto.ProfileImageUrl;
            if (dto.IsAvailable.HasValue) guide.IsAvailable = dto.IsAvailable.Value;

            guide.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new GuideProfileDto
            {
                Id = guide.Id,
                UserId = guide.UserId,
                AttractionId = guide.AttractionId,
                AttractionName = guide.Attraction.Name,
                FullName = guide.FullName,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber,
                Experience = guide.Experience,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                Availability = guide.Availability,
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable
            };
        }

        public async Task<bool> IsGuideAvailableAsync(int guideId, DateTime date, string timeFrom, string timeTo)
        {
            // Check for conflicting bookings
            var hasConflict = await _context.Bookings
                .AnyAsync(b =>
                    b.GuideId == guideId &&
                    b.BookingDate.Date == date.Date &&
                    b.Status != "cancelled" &&
                    (
                        (b.TimeFrom.CompareTo(timeFrom) <= 0 && b.TimeTo.CompareTo(timeFrom) > 0) ||
                        (b.TimeFrom.CompareTo(timeTo) < 0 && b.TimeTo.CompareTo(timeTo) >= 0) ||
                        (b.TimeFrom.CompareTo(timeFrom) >= 0 && b.TimeTo.CompareTo(timeTo) <= 0)
                    )
                );

            return !hasConflict;
        }
    }
}

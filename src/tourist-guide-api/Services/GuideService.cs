using System;
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

        public async Task<IEnumerable<GuideProfileDto>> GetGuidesByAttractionIdAsync(int attractionId, DateTime? fromDate = null, string? timeFrom = null, string? timeTo = null)
        {
            var query = _context.GuideProfiles
                .Include(g => g.Attraction)
                .Include(g => g.AvailableDates)
                .Where(g => g.AttractionId == attractionId && g.IsAvailable);

            var guides = await query.ToListAsync();

            // If time filtering is required, check availability
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                var availableGuides = new List<GuideProfile>();
                foreach (var guide in guides)
                {
                    if (await IsGuideAvailableAsync(guide.Id, fromDate.HasValue ? fromDate.Value : DateTime.Today, timeFrom, timeTo))
                    {
                        availableGuides.Add(guide);
                    }
                }
                guides = availableGuides;
            }

            //var profileURL = _context.Users.Where(u => u.Id == )

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
                TourDuration = g.TourDuration,
                Languages = g.Languages,
                Bio = g.Bio,
                Rating = g.Rating,
                PricePerHour = g.PricePerHour,
                AvailableDates = g.AvailableDates.Select(d => new AvailableDateRangeDto
                {
                    From = d.FromDate,
                    To = d.ToDate
                }).ToList(),
                ProfileImageUrl = _context.Users.Where(u => u.Id == g.UserId).FirstOrDefault().ProfileImageUrl,
                IsAvailable = g.IsAvailable,
                Location = g.Attraction.Location
            });
        }

        public async Task<GuideProfileDto?> GetGuideProfileByIdAsync(int id)
        {
            var guide = await _context.GuideProfiles
                .Include(g => g.Attraction)
                .Include(g => g.AvailableDates)
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
                TourDuration = guide.TourDuration,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                AvailableDates = guide.AvailableDates.Select(d => new AvailableDateRangeDto
                {
                    From = d.FromDate,
                    To = d.ToDate
                }).ToList(),
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable,
                Location = guide.Attraction.Location
            };
        }

        public async Task<IEnumerable<GuideProfileDto?>> GetGuideProfileByUserIdAsync(int userId)
        {
            var guideProfileResults = new List<GuideProfileDto>();
            var guideProfiles = await _context.GuideProfiles
                .Include(g => g.Attraction)
                .Include(g => g.AvailableDates)
                .Where(g => g.UserId == userId).ToListAsync();

            if (guideProfiles == null || guideProfiles.Count == 0) return [];

            foreach (var guide in guideProfiles)
            {
                var guideProfile = new GuideProfileDto
                {
                    Id = guide.Id,
                    UserId = guide.UserId,
                    AttractionId = guide.AttractionId,
                    AttractionName = guide.Attraction.Name,
                    FullName = guide.FullName,
                    Email = guide.Email,
                    PhoneNumber = guide.PhoneNumber,
                    Experience = guide.Experience,
                    TourDuration = guide.TourDuration,
                    Languages = guide.Languages,
                    Bio = guide.Bio,
                    Rating = guide.Rating,
                    PricePerHour = guide.PricePerHour,
                    AvailableDates = guide.AvailableDates.Select(d => new AvailableDateRangeDto
                    {
                        From = d.FromDate,
                        To = d.ToDate
                    }).ToList(),
                    ProfileImageUrl = _context.Users.Where(u => u.Id == guide.UserId).FirstOrDefault().ProfileImageUrl,
                    IsAvailable = guide.IsAvailable,
                    Location = guide.Attraction.Location
                };

                guideProfileResults.Add(guideProfile);
            }

            return guideProfileResults;
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
                Experience = dto.Experience ?? 0,
                TourDuration = dto.TourDuration,
                Languages = dto.Languages,
                Bio = dto.Bio,
                Rating = dto.Rating == 0 ? 4 : dto.Rating,
                PricePerHour = dto.PricePerHour,
                ProfileImageUrl = dto.ProfileImageUrl,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.GuideProfiles.Add(guide);
            await _context.SaveChangesAsync();

            // Add available dates
            if (dto.AvailableDates != null && dto.AvailableDates.Any())
            {
                var availableDates = dto.AvailableDates.Select(d => new GuideAvailableDate
                {
                    GuideProfileId = guide.Id,
                    FromDate = d.From,
                    ToDate = d.To,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                _context.GuideAvailableDates.AddRange(availableDates);
                await _context.SaveChangesAsync();
            }

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
                TourDuration = guide.TourDuration,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                AvailableDates = dto.AvailableDates ?? new List<AvailableDateRangeDto>(),
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable,
                Location = guide.Attraction.Location
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
            if (dto.TourDuration > 0) guide.TourDuration = dto.TourDuration;
            if (dto.Languages != null) guide.Languages = dto.Languages;
            if (dto.Bio != null) guide.Bio = dto.Bio;
            if (dto.Rating.HasValue) guide.Rating = dto.Rating.Value;
            if (dto.PricePerHour.HasValue) guide.PricePerHour = dto.PricePerHour.Value;
            if (dto.ProfileImageUrl != null) guide.ProfileImageUrl = dto.ProfileImageUrl;
            if (dto.IsAvailable.HasValue) guide.IsAvailable = dto.IsAvailable.Value;

            // Update available dates if provided
            if (dto.AvailableDates != null && dto.AvailableDates.Any())
            {
                // Remove existing dates
                var existingDates = await _context.GuideAvailableDates
                    .Where(d => d.GuideProfileId == guide.Id)
                    .ToListAsync();
                _context.GuideAvailableDates.RemoveRange(existingDates);

                // Add new dates
                var newDates = dto.AvailableDates.Select(d => new GuideAvailableDate
                {
                    GuideProfileId = guide.Id,
                    FromDate = d.From,
                    ToDate = d.To,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                _context.GuideAvailableDates.AddRange(newDates);
            }

            guide.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Reload to get updated AvailableDates
            await _context.Entry(guide).Collection(g => g.AvailableDates).LoadAsync();

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
                TourDuration = guide.TourDuration,
                Languages = guide.Languages,
                Bio = guide.Bio,
                Rating = guide.Rating,
                PricePerHour = guide.PricePerHour,
                AvailableDates = guide.AvailableDates.Select(d => new AvailableDateRangeDto
                {
                    From = d.FromDate,
                    To = d.ToDate
                }).ToList(),
                ProfileImageUrl = guide.ProfileImageUrl,
                IsAvailable = guide.IsAvailable,
                Location = guide.Attraction.Location
            };
        }

        public async Task<bool> IsGuideAvailableAsync(int guideId, DateTime date, string timeFrom, string timeTo)
        {
            var isAvailable = await _context.GuideAvailableDates
                .AnyAsync(d =>
                    d.GuideProfileId == guideId &&
                    d.FromDate.Date <= date.Date &&
                    d.ToDate.Date >= date.Date);

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

            return isAvailable && !hasConflict;
        }
    }
}

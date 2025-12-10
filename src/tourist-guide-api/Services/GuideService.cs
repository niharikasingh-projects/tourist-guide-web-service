using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TouristGuide.API.Data;
using TouristGuide.API.DTOs;

namespace TouristGuide.API.Services
{
    public class GuideService : IGuideService
    {
        private readonly TouristGuideDbContext _context;

        public GuideService(TouristGuideDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuideResponse>> GetGuidesByAttractionIdAsync(string attractionId, string? fromDate, string? toDate)
        {
            var guides = await _context.Guides
                .Where(g => g.AttractionIds.Contains(attractionId))
                .ToListAsync();

            var guideResponses = guides.Select(MapToGuideResponse).ToList();

            // Filter by date if provided
            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                var requestFrom = DateTime.Parse(fromDate);
                var requestTo = DateTime.Parse(toDate);

                guideResponses = guideResponses.Where(g => 
                    g.AvailableDates.Any(dateRange =>
                    {
                        var availableFrom = DateTime.Parse(dateRange.From);
                        var availableTo = DateTime.Parse(dateRange.To);
                        return requestFrom <= availableTo && requestTo >= availableFrom;
                    })
                ).ToList();
            }

            return guideResponses;
        }

        public async Task<GuideResponse?> GetGuideByIdAsync(string guideId)
        {
            var guide = await _context.Guides.FindAsync(guideId);
            return guide != null ? MapToGuideResponse(guide) : null;
        }

        private GuideResponse MapToGuideResponse(Models.Guide guide)
        {
            var availableDates = new List<DateRange>();
            try
            {
                var dates = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(guide.AvailableDates);
                if (dates != null)
                {
                    availableDates = dates.Select(d => new DateRange
                    {
                        From = d.ContainsKey("from") ? d["from"] : "",
                        To = d.ContainsKey("to") ? d["to"] : ""
                    }).ToList();
                }
            }
            catch
            {
                // If parsing fails, return empty list
            }

            return new GuideResponse
            {
                Id = guide.Id,
                Name = guide.Name,
                ImageUrl = guide.ImageUrl,
                Languages = guide.Languages.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Availability = guide.Availability,
                HourlyRate = guide.HourlyRate,
                Rating = guide.Rating,
                ExperienceYears = guide.ExperienceYears,
                Specialties = guide.Specialties.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Bio = guide.Bio,
                AvailableDates = availableDates,
                Contact = guide.Contact,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber
            };
        }
    }
}

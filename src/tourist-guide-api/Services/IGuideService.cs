using TouristGuide.API.DTOs;

namespace TouristGuide.API.Services
{
    public interface IGuideService
    {
        Task<IEnumerable<GuideResponse>> GetGuidesByAttractionIdAsync(string attractionId, string? fromDate, string? toDate);
        Task<GuideResponse?> GetGuideByIdAsync(string guideId);
    }
}

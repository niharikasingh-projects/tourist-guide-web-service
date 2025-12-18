using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public interface IGuideService
    {
        Task<IEnumerable<GuideProfileDto>> GetGuidesByAttractionIdAsync(int attractionId, string? timeFrom = null, string? timeTo = null);
        Task<GuideProfileDto?> GetGuideProfileByIdAsync(int id);
        Task<GuideProfileDto?> GetGuideProfileByUserIdAsync(int userId);
        Task<GuideProfileDto> CreateGuideProfileAsync(int userId, CreateGuideProfileDto dto);
        Task<GuideProfileDto?> UpdateGuideProfileAsync(int userId, UpdateGuideProfileDto dto);
        Task<bool> IsGuideAvailableAsync(int guideId, DateTime date, string timeFrom, string timeTo);
    }
}

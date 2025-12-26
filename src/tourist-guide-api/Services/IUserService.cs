using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
        Task<UserProfileDto?> UpdateUserProfileAsync(int userId, UpdateUserProfileDto dto);
    }
}

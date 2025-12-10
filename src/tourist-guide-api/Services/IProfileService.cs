using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public interface IProfileService
    {
        Task<UserProfile?> GetUserProfileAsync(string email);
        Task<UserProfile> UpdateUserProfileAsync(UserProfile profile);
        Task<bool> DeleteUserProfileAsync(string email);
    }
}

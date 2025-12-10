using Microsoft.EntityFrameworkCore;
using TouristGuide.API.Data;
using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly TouristGuideDbContext _context;

        public ProfileService(TouristGuideDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetUserProfileAsync(string email)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<UserProfile> UpdateUserProfileAsync(UserProfile profile)
        {
            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.Email == profile.Email);

            if (existingProfile != null)
            {
                existingProfile.Name = profile.Name;
                existingProfile.DateOfBirth = profile.DateOfBirth;
                existingProfile.PhoneNumber = profile.PhoneNumber;
                existingProfile.Country = profile.Country;
                
                await _context.SaveChangesAsync();
                return existingProfile;
            }
            else
            {
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
                return profile;
            }
        }

        public async Task<bool> DeleteUserProfileAsync(string email)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Email == email);
            if (profile == null) return false;

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

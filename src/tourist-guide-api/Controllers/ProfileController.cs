using Microsoft.AspNetCore.Mvc;
using TouristGuide.API.Models;
using TouristGuide.API.Services;

namespace TouristGuide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserProfile(string email)
        {
            var profile = await _profileService.GetUserProfileAsync(email);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfile profile)
        {
            var updatedProfile = await _profileService.UpdateUserProfileAsync(profile);
            return Ok(updatedProfile);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserProfile(string email)
        {
            var success = await _profileService.DeleteUserProfileAsync(email);
            if (!success)
                return NotFound();

            return Ok(new { success = true });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Services;

namespace TouristGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var profile = await _userService.GetUserProfileAsync(userId);

                if (profile == null)
                    return NotFound(new { message = "User not found" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var profile = await _userService.GetUserProfileAsync(id);

                if (profile == null)
                    return NotFound(new { message = "User not found" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateUserProfileDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var profile = await _userService.UpdateUserProfileAsync(userId, dto);

                if (profile == null)
                    return NotFound(new { message = "User not found" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("profile")]
        public async Task<IActionResult> PatchMyProfile([FromBody] UpdateUserProfileDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var profile = await _userService.UpdateUserProfileAsync(userId, dto);

                if (profile == null)
                    return NotFound(new { message = "User not found" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

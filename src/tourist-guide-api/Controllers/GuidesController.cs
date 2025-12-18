using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Services;

namespace TouristGuide.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuidesController : ControllerBase
    {
        private readonly IGuideService _guideService;

        public GuidesController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        [HttpGet("attraction/{attractionId}")]
        public async Task<IActionResult> GetByAttraction(int attractionId, [FromQuery] string? timeFrom = null, [FromQuery] string? timeTo = null)
        {
            var guides = await _guideService.GetGuidesByAttractionIdAsync(attractionId, timeFrom, timeTo);
            return Ok(guides);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guide = await _guideService.GetGuideProfileByIdAsync(id);
            if (guide == null)
                return NotFound(new { message = "Guide not found" });

            return Ok(guide);
        }

        [Authorize(Roles = "guide")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var guide = await _guideService.GetGuideProfileByUserIdAsync(userId);

            if (guide == null)
                return NotFound(new { message = "Guide profile not found" });

            return Ok(guide);
        }

        [Authorize(Roles = "guide")]
        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateGuideProfileDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var guide = await _guideService.CreateGuideProfileAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = guide.Id }, guide);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "guide")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateGuideProfileDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var guide = await _guideService.UpdateGuideProfileAsync(userId, dto);

                if (guide == null)
                    return NotFound(new { message = "Guide profile not found" });

                return Ok(guide);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> CheckAvailability(int id, [FromQuery] DateTime date, [FromQuery] string timeFrom, [FromQuery] string timeTo)
        {
            var isAvailable = await _guideService.IsGuideAvailableAsync(id, date, timeFrom, timeTo);
            return Ok(new { isAvailable });
        }
    }
}

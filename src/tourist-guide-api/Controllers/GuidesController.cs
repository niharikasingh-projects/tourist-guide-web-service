using Microsoft.AspNetCore.Mvc;
using TouristGuide.API.Services;

namespace TouristGuide.API.Controllers
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

        [HttpGet("by-attraction/{attractionId}")]
        public async Task<IActionResult> GetGuidesByAttractionId(string attractionId, [FromQuery] string? fromDate, [FromQuery] string? toDate)
        {
            var guides = await _guideService.GetGuidesByAttractionIdAsync(attractionId, fromDate, toDate);
            return Ok(guides);
        }

        [HttpGet("{guideId}")]
        public async Task<IActionResult> GetGuideById(string guideId)
        {
            var guide = await _guideService.GetGuideByIdAsync(guideId);
            if (guide == null)
                return NotFound();

            return Ok(guide);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TouristGuide.API.Services;

namespace TouristGuide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionService _attractionService;

        public AttractionsController(IAttractionService attractionService)
        {
            _attractionService = attractionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttractions()
        {
            var attractions = await _attractionService.GetAllAttractionsAsync();
            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttractionById(string id)
        {
            var attraction = await _attractionService.GetAttractionByIdAsync(id);
            if (attraction == null)
                return NotFound();

            return Ok(attraction);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAttractions([FromQuery] string? location, [FromQuery] string? fromDate, [FromQuery] string? toDate)
        {
            var attractions = await _attractionService.SearchAttractionsAsync(location, fromDate, toDate);
            return Ok(attractions);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Services;

namespace TouristGuide.Api.Controllers
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
        public async Task<IActionResult> GetAll()
        {
            var attractions = await _attractionService.GetAllAttractionsAsync();
            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attraction = await _attractionService.GetAttractionByIdAsync(id);
            if (attraction == null)
                return NotFound(new { message = "Attraction not found" });

            return Ok(attraction);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string location)
        {
            var attractions = await _attractionService.SearchAttractionsAsync(location);
            return Ok(attractions);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateAttractionDto dto)
        {
            try
            {
                var attraction = await _attractionService.CreateAttractionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = attraction.Id }, attraction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateAttractionDto dto)
        {
            try
            {
                var attraction = await _attractionService.UpdateAttractionAsync(id, dto);
                if (attraction == null)
                    return NotFound(new { message = "Attraction not found" });

                return Ok(attraction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attractionService.DeleteAttractionAsync(id);
            if (!result)
                return NotFound(new { message = "Attraction not found" });

            return NoContent();
        }

        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetByLocation(string location)
        {
            var attractions = await _attractionService.GetAttractionsByLocationAsync(location);
            return Ok(attractions);
        }
    }
}

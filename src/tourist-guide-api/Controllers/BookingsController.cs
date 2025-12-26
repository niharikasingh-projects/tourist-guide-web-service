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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        //[Authorize(Roles = "tourist")]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var booking = await _bookingService.CreateBookingAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Check authorization
            if (role == "tourist" && booking.UserId != userId)
                return Forbid();

            return Ok(booking);
        }

        [HttpGet("my-bookings/{id}")]
        //[Authorize(Roles = "tourist")]
        public async Task<IActionResult> GetMyBookings(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("guide-bookings")]
        [Authorize(Roles = "guide")]
        public async Task<IActionResult> GetGuideBookings([FromQuery] int guideId)
        {
            var bookings = await _bookingService.GetGuideBookingsAsync(guideId);
            return Ok(bookings);
        }

        [HttpGet("guide-bookings/categorize")]
        [Authorize(Roles = "guide")]
        public async Task<IActionResult> CategorizeGuideBookings([FromQuery] int guideId, [FromQuery] DateTime selectedDate)
        {
            var result = await _bookingService.CategorizeGuideBookingsAsync(guideId, selectedDate);
            return Ok(new
            {
                current = result.current,
                past = result.past,
                future = result.future
            });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusDto dto)
        {
            try
            {
                var booking = await _bookingService.UpdateBookingStatusAsync(id, dto.Status);
                if (booking == null)
                    return NotFound(new { message = "Booking not found" });

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

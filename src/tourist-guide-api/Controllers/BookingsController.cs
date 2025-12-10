using Microsoft.AspNetCore.Mvc;
using TouristGuide.API.DTOs;
using TouristGuide.API.Services;

namespace TouristGuide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var booking = await _bookingService.CreateBookingAsync(request);
            return Ok(booking);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("by-customer/{email}")]
        public async Task<IActionResult> GetBookingsByCustomerEmail(string email)
        {
            var bookings = await _bookingService.GetBookingsByCustomerEmailAsync(email);
            return Ok(bookings);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(string id)
        {
            var success = await _bookingService.CancelBookingAsync(id);
            if (!success)
                return NotFound();

            return Ok(new { success = true });
        }
    }
}

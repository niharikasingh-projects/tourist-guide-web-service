using Microsoft.EntityFrameworkCore;
using TouristGuide.API.Data;
using TouristGuide.API.DTOs;
using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly TouristGuideDbContext _context;
        private static int _bookingCounter = 1000;

        public BookingService(TouristGuideDbContext context)
        {
            _context = context;
        }

        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
        {
            var booking = new Booking
            {
                Id = $"BK-{++_bookingCounter}",
                AttractionId = request.AttractionId,
                AttractionName = request.AttractionName,
                GuideId = request.GuideId,
                GuideName = request.GuideName,
                GuideContact = request.GuideContact,
                GuideEmail = request.GuideEmail,
                CustomerName = request.CustomerName,
                CustomerContact = request.CustomerContact,
                CustomerEmail = request.CustomerEmail,
                Amount = request.Amount,
                BookingDate = DateTime.UtcNow,
                Status = "confirmed",
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return MapToBookingResponse(booking);
        }

        public async Task<BookingResponse?> GetBookingByIdAsync(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            return booking != null ? MapToBookingResponse(booking) : null;
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsByCustomerEmailAsync(string email)
        {
            var bookings = await _context.Bookings
                .Where(b => b.CustomerEmail == email)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return bookings.Select(MapToBookingResponse);
        }

        public async Task<bool> CancelBookingAsync(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            booking.Status = "cancelled";
            await _context.SaveChangesAsync();

            return true;
        }

        private BookingResponse MapToBookingResponse(Booking booking)
        {
            return new BookingResponse
            {
                Id = booking.Id,
                AttractionId = booking.AttractionId,
                AttractionName = booking.AttractionName,
                GuideId = booking.GuideId,
                GuideName = booking.GuideName,
                GuideContact = booking.GuideContact,
                GuideEmail = booking.GuideEmail,
                CustomerName = booking.CustomerName,
                CustomerContact = booking.CustomerContact,
                CustomerEmail = booking.CustomerEmail,
                Amount = booking.Amount,
                BookingDate = booking.BookingDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Status = booking.Status,
                FromDate = booking.FromDate,
                ToDate = booking.ToDate
            };
        }
    }
}

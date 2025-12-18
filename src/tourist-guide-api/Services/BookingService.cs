using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingDto> CreateBookingAsync(int userId, CreateBookingDto dto)
        {
            var guide = await _context.GuideProfiles.FindAsync(dto.GuideId);
            if (guide == null)
            {
                throw new Exception("Guide not found");
            }

            // Calculate hours
            var timeFrom = TimeSpan.Parse(dto.TimeFrom);
            var timeTo = TimeSpan.Parse(dto.TimeTo);
            var hours = (decimal)(timeTo - timeFrom).TotalHours;

            // Calculate amounts
            var totalAmount = hours * guide.PricePerHour * dto.NumberOfPeople;
            var taxAmount = totalAmount * 0.28m; // 28% tax (14% CGST + 14% SGST)
            var grandTotal = totalAmount + taxAmount;

            var booking = new Booking
            {
                UserId = userId,
                GuideId = dto.GuideId,
                AttractionId = dto.AttractionId,
                BookingDate = dto.BookingDate,
                TimeFrom = dto.TimeFrom,
                TimeTo = dto.TimeTo,
                NumberOfPeople = dto.NumberOfPeople,
                TotalAmount = totalAmount,
                TaxAmount = taxAmount,
                GrandTotal = grandTotal,
                Status = "pending",
                TouristName = dto.TouristName,
                TouristEmail = dto.TouristEmail,
                TouristPhone = dto.TouristPhone,
                SpecialRequests = dto.SpecialRequests,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var bookingDto = await GetBookingByIdAsync(booking.Id);
            return bookingDto!;
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Guide)
                .Include(b => b.Attraction)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GuideId = b.GuideId,
                    AttractionId = b.AttractionId,
                    AttractionName = b.Attraction.Name,
                    GuideName = b.Guide.FullName,
                    BookingDate = b.BookingDate,
                    TimeFrom = b.TimeFrom,
                    TimeTo = b.TimeTo,
                    NumberOfPeople = b.NumberOfPeople,
                    TotalAmount = b.TotalAmount,
                    TaxAmount = b.TaxAmount,
                    GrandTotal = b.GrandTotal,
                    Status = b.Status,
                    TouristName = b.TouristName,
                    TouristEmail = b.TouristEmail,
                    TouristPhone = b.TouristPhone,
                    SpecialRequests = b.SpecialRequests,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDto>> GetGuideBookingsAsync(int guideId)
        {
            return await _context.Bookings
                .Include(b => b.Guide)
                .Include(b => b.Attraction)
                .Where(b => b.GuideId == guideId)
                .OrderByDescending(b => b.BookingDate)
                .ThenBy(b => b.TimeFrom)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GuideId = b.GuideId,
                    AttractionId = b.AttractionId,
                    AttractionName = b.Attraction.Name,
                    GuideName = b.Guide.FullName,
                    BookingDate = b.BookingDate,
                    TimeFrom = b.TimeFrom,
                    TimeTo = b.TimeTo,
                    NumberOfPeople = b.NumberOfPeople,
                    TotalAmount = b.TotalAmount,
                    TaxAmount = b.TaxAmount,
                    GrandTotal = b.GrandTotal,
                    Status = b.Status,
                    TouristName = b.TouristName,
                    TouristEmail = b.TouristEmail,
                    TouristPhone = b.TouristPhone,
                    SpecialRequests = b.SpecialRequests,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Guide)
                .Include(b => b.Attraction)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return null;

            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                GuideId = booking.GuideId,
                AttractionId = booking.AttractionId,
                AttractionName = booking.Attraction.Name,
                GuideName = booking.Guide.FullName,
                BookingDate = booking.BookingDate,
                TimeFrom = booking.TimeFrom,
                TimeTo = booking.TimeTo,
                NumberOfPeople = booking.NumberOfPeople,
                TotalAmount = booking.TotalAmount,
                TaxAmount = booking.TaxAmount,
                GrandTotal = booking.GrandTotal,
                Status = booking.Status,
                TouristName = booking.TouristName,
                TouristEmail = booking.TouristEmail,
                TouristPhone = booking.TouristPhone,
                SpecialRequests = booking.SpecialRequests,
                CreatedAt = booking.CreatedAt
            };
        }

        public async Task<BookingDto?> UpdateBookingStatusAsync(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return null;

            booking.Status = status;
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetBookingByIdAsync(id);
        }

        public async Task<(IEnumerable<BookingDto> current, IEnumerable<BookingDto> past, IEnumerable<BookingDto> future)>
            CategorizeGuideBookingsAsync(int guideId, DateTime selectedDate)
        {
            var allBookings = await GetGuideBookingsAsync(guideId);

            var current = allBookings.Where(b =>
                b.BookingDate.Date == selectedDate.Date &&
                (b.Status == "confirmed" || b.Status == "pending"));

            var past = allBookings.Where(b =>
                b.BookingDate.Date < selectedDate.Date ||
                (b.BookingDate.Date == selectedDate.Date && b.Status == "completed"));

            var future = allBookings.Where(b =>
                b.BookingDate.Date > selectedDate.Date &&
                (b.Status == "confirmed" || b.Status == "pending"));

            return (current, past, future);
        }
    }
}

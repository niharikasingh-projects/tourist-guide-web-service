using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public BookingService(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<BookingDto> CreateBookingAsync(int userId, CreateBookingDto dto)
        {
            // Validate booking date
            if (dto.BookingDate == default || dto.BookingDate.Year < DateTime.UtcNow.Year)
            {
                throw new Exception("Invalid booking date. Please provide a valid date.");
            }

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
                    GuideContact = b.Guide.PhoneNumber,
                    GuideEmail = b.Guide.Email,
                    BookingDate = b.BookingDate,
                    SelectedDate = b.BookingDate,
                    TimeFrom = b.TimeFrom,
                    TimeTo = b.TimeTo,
                    //NumberOfPeople = b.NumberOfPeople,
                    TotalAmount = b.TotalAmount,
                    TaxAmount = b.TaxAmount,
                    GrandTotal = b.GrandTotal,
                    Status = b.Status,
                    TouristName = b.TouristName,
                    TouristEmail = b.TouristEmail,
                    TouristPhone = b.TouristPhone,
                    SpecialRequests = b.SpecialRequests,
                    CreatedAt = b.CreatedAt,
                    PaymentMethod = b.Payment.PaymentMethod,
                    PaymentStatus = b.Payment.Status,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDto>> GetGuideBookingsAsync(int guideId)
        {
            return await _context.Bookings
                .Include(b => b.Guide)
                .Include(b => b.Attraction)
                .Include(b => b.Payment)
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
                    GuideContact = b.Guide.PhoneNumber,
                    GuideEmail = b.Guide.Email,
                    BookingDate = b.BookingDate,
                    SelectedDate = b.BookingDate,
                    TimeFrom = b.TimeFrom,
                    TimeTo = b.TimeTo,
                    //NumberOfPeople = b.NumberOfPeople,
                    TotalAmount = b.TotalAmount,
                    TaxAmount = b.TaxAmount,
                    GrandTotal = b.GrandTotal,
                    Status = b.Status,
                    TouristName = b.TouristName,
                    TouristEmail = b.TouristEmail,
                    TouristPhone = b.TouristPhone,
                    SpecialRequests = b.SpecialRequests,
                    CreatedAt = b.CreatedAt,
                    PaymentMethod = b.Payment.PaymentMethod,
                    PaymentStatus = b.Payment.Status,
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Guide)
                .Include(b => b.Attraction)
                .Include(b => b.Payment)
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
                SelectedDate = booking.BookingDate,
                GuideContact = booking.Guide.PhoneNumber,
                GuideEmail = booking.Guide.Email,
                TimeFrom = booking.TimeFrom,
                TimeTo = booking.TimeTo,
                //NumberOfPeople = booking.NumberOfPeople,
                TotalAmount = booking.TotalAmount,
                TaxAmount = booking.TaxAmount,
                GrandTotal = booking.GrandTotal,
                Status = booking.Status,
                TouristName = booking.TouristName,
                TouristEmail = booking.TouristEmail,
                TouristPhone = booking.TouristPhone,
                SpecialRequests = booking.SpecialRequests,
                PaymentMethod = booking?.Payment?.PaymentMethod ?? string.Empty,
                PaymentStatus = booking?.Payment?.Status ?? string.Empty,
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

        public async Task<BookingDto?> CancelBookingAsync(int id, int userId)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return null;

            // Verify the user owns this booking
            if (booking.UserId != userId && booking.GuideId != userId)
            {
                throw new UnauthorizedAccessException("You can only cancel your own bookings");
            }

            // Check if booking can be cancelled
            if (booking.Status == "cancelled")
            {
                throw new Exception("Booking is already cancelled");
            }

            if (booking.Status == "completed")
            {
                throw new Exception("Cannot cancel a completed booking");
            }

            // Update booking status to cancelled
            booking.Status = "cancelled";
            booking.UpdatedAt = DateTime.UtcNow;

            // Update payment status to refunded if payment exists
            await _paymentService.UpdatePaymentStatusAsync(booking.Id, "refunded");

            await _context.SaveChangesAsync();

            return await GetBookingByIdAsync(id);
        }

        public async Task<(IEnumerable<BookingDto> current, IEnumerable<BookingDto> past, IEnumerable<BookingDto> future)>
            CategorizeGuideBookingsAsync(int guideId, DateTime selectedDate)
        {
            var allBookings = await GetGuideBookingsAsync(guideId);

            var current = allBookings.Where(b =>
                (b.BookingDate).Date == selectedDate.Date &&
                (b.Status == "confirmed" || b.Status == "pending"));

            var past = allBookings.Where(b =>
                (b.BookingDate).Date < selectedDate.Date ||
                ((b.BookingDate).Date == selectedDate.Date && b.Status == "completed"));

            var future = allBookings.Where(b =>
                (b.BookingDate).Date > selectedDate.Date &&
                (b.Status == "confirmed" || b.Status == "pending"));

            return (current, past, future);
        }
    }
}

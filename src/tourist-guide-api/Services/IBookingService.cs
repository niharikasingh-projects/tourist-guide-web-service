using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(int userId, CreateBookingDto dto);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId);
        Task<IEnumerable<BookingDto>> GetGuideBookingsAsync(int guideId);
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<BookingDto?> UpdateBookingStatusAsync(int id, string status);
        Task<BookingDto?> CancelBookingAsync(int id, int userId);
        Task<(IEnumerable<BookingDto> current, IEnumerable<BookingDto> past, IEnumerable<BookingDto> future)>
            CategorizeGuideBookingsAsync(int guideId, DateTime selectedDate);
    }
}

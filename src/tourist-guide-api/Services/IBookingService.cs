using TouristGuide.API.DTOs;

namespace TouristGuide.API.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
        Task<BookingResponse?> GetBookingByIdAsync(string id);
        Task<IEnumerable<BookingResponse>> GetBookingsByCustomerEmailAsync(string email);
        Task<bool> CancelBookingAsync(string id);
    }
}

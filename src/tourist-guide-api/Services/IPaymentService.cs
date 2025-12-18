using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ProcessPaymentAsync(ProcessPaymentDto dto);
        Task<PaymentDto?> GetPaymentByBookingIdAsync(int bookingId);
    }
}

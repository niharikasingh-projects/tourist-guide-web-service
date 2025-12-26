using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> ProcessPaymentAsync(ProcessPaymentDto dto)
        {
            var booking = await _context.Bookings.FindAsync(dto.BookingId);
            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            // Check if payment already exists
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == dto.BookingId);

            if (existingPayment != null)
            {
                throw new Exception("Payment already processed for this booking");
            }

            // Validate payment method specific details
            if (string.Equals(dto.PaymentMethod, "UPI", StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrEmpty(dto.UpiId))
            {
                throw new Exception("UPI ID is required for UPI payment");
            }

            if (string.Equals(dto.PaymentMethod, "CreditCard", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(dto.CardNumber) || string.IsNullOrEmpty(dto.CardHolderName))
                {
                    throw new Exception("Card details are required for Credit Card payment");
                }

                // Test card validation (1111 1111 1111 1111)
                var cardNumber = dto.CardNumber.Replace(" ", "");
                if (cardNumber == "1111111111111111")
                {
                    // Test card - always succeeds
                }
                else
                {
                    // In production, integrate with actual payment gateway
                    // For now, accept all cards
                }
            }

            var payment = new Payment
            {
                BookingId = dto.BookingId,
                Amount = booking.GrandTotal,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = Guid.NewGuid().ToString().Substring(0, 20),
                Status = string.Equals(dto.PaymentMethod, "PayLater", StringComparison.InvariantCultureIgnoreCase) ? "pending" : "completed",
                UpiId = dto.UpiId,
                CardNumber = dto.CardNumber != null ? "****" + dto.CardNumber.Substring(Math.Max(0, dto.CardNumber.Length - 4)) : null,
                CardHolderName = dto.CardHolderName,
                PaymentDate = !string.Equals(dto.PaymentMethod, "PayLater", StringComparison.InvariantCultureIgnoreCase) ? DateTime.UtcNow : (DateTime?)null,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            //// Update booking status
            //if (dto.PaymentMethod != "PayLater")
            //{
            //    booking.Status = "confirmed";
            //    booking.UpdatedAt = DateTime.UtcNow;
            //}

            booking.Status = "confirmed";
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<PaymentDto?> GetPaymentByBookingIdAsync(int bookingId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (payment == null) return null;

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<bool> UpdatePaymentStatusAsync(int bookingId, string status)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (payment == null) return false;

            payment.Status = status;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Api.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
    }

    public class ProcessPaymentDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // UPI, CreditCard, PayLater

        // UPI Details
        public string? UpiId { get; set; }

        // Card Details
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Cvv { get; set; }
    }
}

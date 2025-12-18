using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TouristGuide.Api.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // UPI, CreditCard, PayLater

        [StringLength(50)]
        public string? TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "pending"; // pending, completed, failed

        // UPI Details
        [StringLength(100)]
        public string? UpiId { get; set; }

        // Card Details
        [StringLength(20)]
        public string? CardNumber { get; set; }

        [StringLength(100)]
        public string? CardHolderName { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; } = null!;
    }
}

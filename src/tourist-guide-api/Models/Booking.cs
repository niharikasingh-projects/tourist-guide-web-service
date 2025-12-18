using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TouristGuide.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GuideId { get; set; }

        [Required]
        public int AttractionId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [StringLength(10)]
        public string TimeFrom { get; set; } = string.Empty; // e.g., "09:00"

        [Required]
        [StringLength(10)]
        public string TimeTo { get; set; } = string.Empty; // e.g., "12:00"

        public int NumberOfPeople { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal GrandTotal { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "pending"; // pending, confirmed, completed, cancelled

        [Required]
        [StringLength(100)]
        public string TouristName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string TouristEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string TouristPhone { get; set; } = string.Empty;

        [StringLength(500)]
        public string? SpecialRequests { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("GuideId")]
        public virtual GuideProfile Guide { get; set; } = null!;

        [ForeignKey("AttractionId")]
        public virtual TouristAttraction Attraction { get; set; } = null!;

        public virtual Payment? Payment { get; set; }
    }
}

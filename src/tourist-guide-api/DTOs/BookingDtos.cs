using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Api.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GuideId { get; set; }
        public int AttractionId { get; set; }
        public string AttractionName { get; set; } = string.Empty;
        public string GuideName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string TimeFrom { get; set; } = string.Empty;
        public string TimeTo { get; set; } = string.Empty;
        public int NumberOfPeople { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TouristName { get; set; } = string.Empty;
        public string TouristEmail { get; set; } = string.Empty;
        public string TouristPhone { get; set; } = string.Empty;
        public string? SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBookingDto
    {
        [Required]
        public int GuideId { get; set; }

        [Required]
        public int AttractionId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeFrom { get; set; } = string.Empty;

        [Required]
        public string TimeTo { get; set; } = string.Empty;

        [Range(1, 50)]
        public int NumberOfPeople { get; set; } = 1;

        [Required]
        [StringLength(100)]
        public string TouristName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string TouristEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string TouristPhone { get; set; } = string.Empty;

        public string? SpecialRequests { get; set; }
    }

    public class UpdateBookingStatusDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty; // confirmed, completed, cancelled
    }
}

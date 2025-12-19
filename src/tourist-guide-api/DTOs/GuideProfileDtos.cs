using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TouristGuide.Api.DTOs
{
    public class GuideProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AttractionId { get; set; }
        public string AttractionName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("experienceYears")]
        public int Experience { get; set; }
        public string Languages { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public decimal Rating { get; set; }

        [JsonPropertyName("hourlyRate")]
        public decimal PricePerHour { get; set; }
        public string Availability { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CreateGuideProfileDto
    {
        [Required]
        public int AttractionId { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("guideName")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [JsonPropertyName("guideEmail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Range(0, 50)]
        public int Experience { get; set; }

        [Required]
        public string Languages { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Bio { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        [JsonPropertyName("hourlyRate")]
        [Range(0, 10000)]
        public decimal PricePerHour { get; set; }

        public string Availability { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ProfileImageUrl { get; set; }
    }

    public class UpdateGuideProfileDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Experience { get; set; }
        public string? Languages { get; set; }
        public string? Bio { get; set; }
        public decimal? Rating { get; set; }
        public decimal? PricePerHour { get; set; }
        public string? Availability { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool? IsAvailable { get; set; }
    }
}

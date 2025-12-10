using System.ComponentModel.DataAnnotations;

namespace TouristGuide.API.Models
{
    public class Guide
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty; // Stored as comma-separated
        public string Availability { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public decimal Rating { get; set; }
        public int ExperienceYears { get; set; }
        public string Specialties { get; set; } = string.Empty; // Stored as comma-separated
        public string Bio { get; set; } = string.Empty;
        public string AvailableDates { get; set; } = string.Empty; // Stored as JSON
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        
        // Navigation property
        public string AttractionIds { get; set; } = string.Empty; // Stored as comma-separated
    }
}

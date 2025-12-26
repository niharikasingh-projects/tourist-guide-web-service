using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Api.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Location { get; set; }
        public string? Languages { get; set; }
        public string? Certifications { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserProfileDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(500)]
        public string? Languages { get; set; }

        [StringLength(500)]
        public string? Certifications { get; set; }
        public IFormFile? ProfileImageUrl { get; set; }
    }
}

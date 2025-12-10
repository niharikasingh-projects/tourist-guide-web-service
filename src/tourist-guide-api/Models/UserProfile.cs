using System.ComponentModel.DataAnnotations;

namespace TouristGuide.API.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
    }
}

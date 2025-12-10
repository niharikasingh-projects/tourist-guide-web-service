using System.ComponentModel.DataAnnotations;

namespace TouristGuide.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // In production, this should be hashed
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

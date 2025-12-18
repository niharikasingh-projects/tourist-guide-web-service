using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Api.Models
{
    public class TouristAttraction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        public decimal EntryFee { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<GuideProfile> GuideProfiles { get; set; } = new List<GuideProfile>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

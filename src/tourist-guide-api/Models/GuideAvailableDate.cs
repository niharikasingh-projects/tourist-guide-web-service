using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TouristGuide.Api.Models
{
    public class GuideAvailableDate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GuideProfileId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("GuideProfileId")]
        public virtual GuideProfile GuideProfile { get; set; } = null!;
    }
}

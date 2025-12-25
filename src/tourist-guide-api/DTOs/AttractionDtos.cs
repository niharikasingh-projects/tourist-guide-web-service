using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TouristGuide.Api.DTOs
{
    public class AttractionDto
    {
        public int Id { get; set; }

        //[JsonPropertyName("attractionName")]
        public string AttractionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public decimal Rating { get; set; }
        public decimal EntryFee { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAttractionDto
    {
        //[JsonPropertyName("attractionName")]
        public string AttractionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        //public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public decimal Rating { get; set; }
        public decimal EntryFee { get; set; }
        public IFormFile? AttractionPicture { get; set; }
    }

    public class UpdateAttractionDto
    {
        //[JsonPropertyName("attractionName")]
        public string? AttractionName { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public decimal? Rating { get; set; }
        public decimal? EntryFee { get; set; }
        public bool? IsActive { get; set; }

        public IFormFile? AttractionPicture { get; set; }
    }
}

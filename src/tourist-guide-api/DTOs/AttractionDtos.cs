namespace TouristGuide.Api.DTOs
{
    public class AttractionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public decimal Rating { get; set; }
        public decimal EntryFee { get; set; }
    }

    public class UpdateAttractionDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public decimal? Rating { get; set; }
        public decimal? EntryFee { get; set; }
        public bool? IsActive { get; set; }
    }
}

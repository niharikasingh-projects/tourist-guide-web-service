namespace TouristGuide.API.DTOs
{
    public class CreateBookingRequest
    {
        public string AttractionId { get; set; } = string.Empty;
        public string AttractionName { get; set; } = string.Empty;
        public string GuideId { get; set; } = string.Empty;
        public string GuideName { get; set; } = string.Empty;
        public string GuideContact { get; set; } = string.Empty;
        public string GuideEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class BookingResponse
    {
        public string Id { get; set; } = string.Empty;
        public string AttractionId { get; set; } = string.Empty;
        public string AttractionName { get; set; } = string.Empty;
        public string GuideId { get; set; } = string.Empty;
        public string GuideName { get; set; } = string.Empty;
        public string GuideContact { get; set; } = string.Empty;
        public string GuideEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string BookingDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}

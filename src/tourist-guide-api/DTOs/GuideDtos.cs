namespace TouristGuide.API.DTOs
{
    public class GuideResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();
        public string Availability { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public decimal Rating { get; set; }
        public int ExperienceYears { get; set; }
        public List<string> Specialties { get; set; } = new();
        public string Bio { get; set; } = string.Empty;
        public List<DateRange> AvailableDates { get; set; } = new();
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class DateRange
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
    }
}

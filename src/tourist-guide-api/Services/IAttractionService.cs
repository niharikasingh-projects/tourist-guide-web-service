using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public interface IAttractionService
    {
        Task<IEnumerable<TouristAttraction>> GetAllAttractionsAsync();
        Task<TouristAttraction?> GetAttractionByIdAsync(string id);
        Task<IEnumerable<TouristAttraction>> SearchAttractionsAsync(string? location, string? fromDate, string? toDate);
    }
}

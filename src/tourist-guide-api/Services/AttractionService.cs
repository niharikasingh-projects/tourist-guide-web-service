using Microsoft.EntityFrameworkCore;
using TouristGuide.API.Data;
using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public class AttractionService : IAttractionService
    {
        private readonly TouristGuideDbContext _context;

        public AttractionService(TouristGuideDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TouristAttraction>> GetAllAttractionsAsync()
        {
            return await _context.TouristAttractions.ToListAsync();
        }

        public async Task<TouristAttraction?> GetAttractionByIdAsync(string id)
        {
            return await _context.TouristAttractions.FindAsync(id);
        }

        public async Task<IEnumerable<TouristAttraction>> SearchAttractionsAsync(string? location, string? fromDate, string? toDate)
        {
            var query = _context.TouristAttractions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(a => 
                    a.Location.Contains(location) || 
                    a.City.Contains(location) || 
                    a.Country.Contains(location));
            }

            return await query.ToListAsync();
        }
    }
}

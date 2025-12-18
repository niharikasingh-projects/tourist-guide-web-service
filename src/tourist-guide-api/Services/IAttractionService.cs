using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Services
{
    public interface IAttractionService
    {
        Task<IEnumerable<AttractionDto>> GetAllAttractionsAsync();
        Task<AttractionDto?> GetAttractionByIdAsync(int id);
        Task<AttractionDto> CreateAttractionAsync(CreateAttractionDto dto);
        Task<AttractionDto?> UpdateAttractionAsync(int id, UpdateAttractionDto dto);
        Task<bool> DeleteAttractionAsync(int id);
        Task<IEnumerable<AttractionDto>> SearchAttractionsAsync(string location);
    }
}

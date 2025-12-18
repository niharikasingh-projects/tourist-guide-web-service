using TouristGuide.Api.DTOs;

namespace TouristGuide.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignUpAsync(SignUpDto signUpDto);
        Task<AuthResponseDto> SignInAsync(SignInDto signInDto);
        string GenerateJwtToken(int userId, string email, string role);
    }
}

using TouristGuide.API.DTOs;

namespace TouristGuide.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> SignInAsync(SignInRequest request);
        Task<AuthResponse> SignUpAsync(SignUpRequest request);
    }
}

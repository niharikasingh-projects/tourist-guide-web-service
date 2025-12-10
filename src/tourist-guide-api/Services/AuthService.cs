using Microsoft.EntityFrameworkCore;
using TouristGuide.API.Data;
using TouristGuide.API.DTOs;
using TouristGuide.API.Models;

namespace TouristGuide.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly TouristGuideDbContext _context;

        public AuthService(TouristGuideDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse> SignInAsync(SignInRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Sign in successful",
                User = new UserData
                {
                    Email = user.Email,
                    Name = user.Name
                }
            };
        }

        public async Task<AuthResponse> SignUpAsync(SignUpRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password, // In production, hash this
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "Account created successfully",
                User = new UserData
                {
                    Email = user.Email,
                    Name = user.Name
                }
            };
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TouristGuide.Api.Data;
using TouristGuide.Api.DTOs;
using TouristGuide.Api.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;

namespace TouristGuide.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> SignUpAsync(SignUpDto signUpDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == signUpDto.Email))
            {
                throw new Exception("Email already registered");
            }

            //Process Profile picture
            var profileUrl = await ProcessProfilePictureAsync(signUpDto);

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);

            // Create user
            var user = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                PasswordHash = passwordHash,
                Role = signUpDto.Role,
                PhoneNumber = signUpDto.PhoneNumber,
                Languages = signUpDto.Languages,
                Location = signUpDto.Location,
                Certifications = signUpDto.Certifications,
                ProfileImageUrl = profileUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token
            var token = GenerateJwtToken(user.Id, user.Email, user.Role);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber
                }
            };
        }

        public async Task<AuthResponseDto> SignInAsync(SignInDto signInDto)
        {
            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == signInDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(signInDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            // Generate token
            var token = GenerateJwtToken(user.Id, user.Email, user.Role);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfileImageUrl
                }
            };
        }

        public string GenerateJwtToken(int userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> ProcessProfilePictureAsync(SignUpDto dto)
        {
            string profileImageUrl = string.Empty;

            // Allowed file types (MIME types)
            var allowedTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif" };
            // Max file size (e.g., 2 MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
            {
                try
                {
                    if (!allowedTypes.Contains(dto.ProfilePicture.ContentType))
                    {
                        throw new Exception("Invalid file type. Only JPG, PNG, and GIF are allowed.");
                    }

                    // Validate file size
                    if (dto.ProfilePicture.Length > maxFileSize)
                    {
                        throw new Exception("File size exceeds 2 MB limit." );
                    }

                    var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "images", "profiles");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProfilePicture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProfilePicture.CopyToAsync(stream);
                    }

                    profileImageUrl = $"/images/profiles/{fileName}";
                }
                catch (IOException)
                {
                    // Log the exception as needed
                    //return StatusCode(500, new { message = "An error occurred while saving the file.", detail = ioEx.Message });
                    throw;
                }
                catch (Exception)
                {
                    // Log the exception as needed
                    //return StatusCode(500, new { message = "Unexpected error during file upload.", detail = ex.Message });
                    throw;
                }
            }

            return profileImageUrl;
        }
    }
}

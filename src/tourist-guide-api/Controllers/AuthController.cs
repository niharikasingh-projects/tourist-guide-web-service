using Microsoft.AspNetCore.Mvc;
using TouristGuide.API.DTOs;
using TouristGuide.API.Services;

namespace TouristGuide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var response = await _authService.SignInAsync(request);
            
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var response = await _authService.SignUpAsync(request);
            
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}

using AssetDAL.DataAccess;
using AssetDAL.Models;
using AssetServiceLayer.DTO;
using AssetServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AssetServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for username: {Username}", request.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request model for username: {Username}", request.Username);
                return BadRequest("Invalid request format.");
            }

            var user = await _userRepository.AuthenticateAsync(request.Username, request.Password);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login failed for username: {Username}. Reason: Invalid credentials or inactive user.", request.Username);
                return Unauthorized("Invalid credentials or inactive user.");
            }

            var token = _tokenService.GenerateToken(user);

            _logger.LogInformation("Login successful for user ID {UserId} ({Username})", user.UserId, user.Username);

            return Ok(new
            {
                token,
                userId = user.UserId,
                username = user.Username,
                role = user.Role
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return NotFound("No user found with this email.");

            // You can generate a reset token or temporary password
            var tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            user.Password = tempPassword;

            var updated = await _userRepository.UpdateAsync(user);
            if (updated != null)
            {
                return Ok(new { message = $"Temporary password: {tempPassword}" });
                // In production, send this password to email
            }

            return StatusCode(500, "Could not reset password.");
        }
    }
}

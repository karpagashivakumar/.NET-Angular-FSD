using AssetDAL.DataAccess;
using AssetDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetServiceLayer.DTO;
using AssetServiceLayer.Services;
using AssetServiceLayer.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace AssetServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        // Admin: Get all users
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Admin requested all users.");

            var users = await _userRepository.GetAllAsync();
            var result = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role,
                IsActive = u.IsActive
            });

            return Ok(result);
        }

        // Admin: Create new user
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            _logger.LogInformation("Admin is creating a new user: {Username}", dto.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid while creating user.");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactNumber = dto.ContactNumber,
                Address = dto.Address,
                Role = dto.Role,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _userRepository.CreateAsync(user);
            _logger.LogInformation("User created successfully with ID {UserId}", created.UserId);

            return Ok(created);
        }

        // Admin: Update user
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UserUpdateDto dto)
        {
            _logger.LogInformation("Admin is updating user ID {UserId}", id);

            if (id != dto.UserId)
            {
                _logger.LogWarning("User ID in route does not match body.");
                return BadRequest("User ID mismatch.");
            }

            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("User not found with ID {UserId}", id);
                return NotFound("User not found.");
            }

            existing.Username = dto.Username;
            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.ContactNumber = dto.ContactNumber;
            existing.Address = dto.Address;
            //existing.IsActive = dto.IsActive;

            var updated = await _userRepository.UpdateAsync(existing);
            _logger.LogInformation("User ID {UserId} updated successfully.", id);

            return Ok(updated);
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                return BadRequest("Invalid user claim");

            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest("Passwords do not match");

            var result = await _userRepository.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            if (!result)
                return BadRequest("Current password is incorrect");

            return Ok("Password changed successfully");
        }


        // Admin: Deactivate user
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(long id)
        {
            _logger.LogInformation("Admin is deactivating user ID {UserId}", id);

            var result = await _userRepository.DeactivateAsync(id);
            if (result)
            {
                _logger.LogInformation("User ID {UserId} deactivated successfully.", id);
                return Ok("User deactivated successfully.");
            }

            _logger.LogWarning("User not found to deactivate with ID {UserId}", id);
            return NotFound("User not found.");
        }

        // Admin: Activate user
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(long id)
        {
            _logger.LogInformation("Admin is activating user ID {UserId}", id);

            var result = await _userRepository.ActivateAsync(id);
            if (result)
            {
                _logger.LogInformation("User ID {UserId} activated successfully.", id);
                return Ok("User activated successfully.");
            }

            _logger.LogWarning("User not found to activate with ID {UserId}", id);
            return NotFound("User not found.");
        }

        // Logged-in user: View own profile
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                _logger.LogWarning("Invalid token claim for current user.");
                return BadRequest("Invalid token claim.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Current user not found with ID {UserId}", userId);
                return NotFound("User not found.");
            }

            _logger.LogInformation("User ID {UserId} retrieved their profile.", userId);
            return Ok(user);
        }

        // Admin or Employee: Get user by ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID {UserId}", id);
                return NotFound();
            }

            return Ok(user);
        }

        // Public: Login to get token
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for username: {Username}", request.Username);

            var user = await _userRepository.AuthenticateAsync(request.Username, request.Password);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login failed for username: {Username}", request.Username);
                return Unauthorized("Invalid credentials or inactive user.");
            }

            var token = _tokenService.GenerateToken(user);
            _logger.LogInformation("Login successful for user ID {UserId}", user.UserId);

            return Ok(new
            {
                token,
                userId = user.UserId,
                username = user.Username,
                role = user.Role
            });
        }

        // Public: Check if username exists
        [AllowAnonymous]
        [HttpGet("check-username")]
        public async Task<IActionResult> UsernameExists([FromQuery] string username)
        {
            _logger.LogInformation("Checking if username exists: {Username}", username);

            var exists = await _userRepository.UsernameExistsAsync(username);
            return Ok(new { exists });
        }

        // Public: Check if email exists
        [AllowAnonymous]
        [HttpGet("check-email")]
        public async Task<IActionResult> EmailExists([FromQuery] string email)
        {
            _logger.LogInformation("Checking if email exists: {Email}", email);

            var exists = await _userRepository.EmailExistsAsync(email);
            return Ok(new { exists });
        }
    }
}

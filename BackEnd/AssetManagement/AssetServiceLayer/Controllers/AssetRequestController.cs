using AssetDAL.DataAccess;
using AssetDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetServiceLayer.Helpers;
using System.Security.Claims;
using AssetServiceLayer.DTO;
using Microsoft.Extensions.Logging;

namespace AssetServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AssetRequestController : ControllerBase
    {
        private readonly IAssetRequestRepository _repository;
        private readonly ILogger<AssetRequestController> _logger;

        public AssetRequestController(IAssetRequestRepository repository, ILogger<AssetRequestController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Admin: View all requests
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Admin requested all asset requests.");

            var data = await _repository.GetAllAsync();

            var result = data.Select(r => new AssetRequestResponseDto
            {
                RequestId = r.RequestId,
                AssetCategory = r.AssetCategory,
                RequestDescription = r.RequestDescription,
                RequestStatus = r.RequestStatus,
                RequestDate = r.RequestDate,
                EmployeeName = $"{r.Employee?.FirstName} {r.Employee?.LastName}",
                RejectionReason = r.RejectionReason
            });

            return Ok(result);
        }

        // Admin: View only pending requests
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            _logger.LogInformation("Admin requested pending asset requests.");

            var data = await _repository.GetPendingAsync();
            return Ok(data);
        }

        // Employee: Create a request (entity version)
        [Authorize(Roles = UserRoles.Employee)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetRequest request)
        {
            _logger.LogInformation("Employee is attempting to create a raw asset request.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating asset request.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                _logger.LogWarning("UserId claim not found in JWT for asset request creation.");
                return Unauthorized("User ID not found in token.");
            }

            request.EmployeeId = long.Parse(userIdClaim.Value);

            var created = await _repository.CreateAsync(request);
            _logger.LogInformation("Asset request created by employee ID {EmployeeId}", request.EmployeeId);
            return Ok(created);
        }

        // Employee: Create a request (DTO version)
        [Authorize]
        [HttpPost("with-dto")]
        public async Task<IActionResult> Create([FromBody] AssetRequestDto dto)
        {
            _logger.LogInformation("DTO-based asset request creation attempt.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model for asset request DTO.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Invalid or missing UserId claim for asset request.");
                return Unauthorized("UserId claim missing or invalid.");
            }

            var request = new AssetRequest
            {
                EmployeeId = employeeId,
                AssetCategory = dto.AssetCategory,
                RequestDescription = dto.RequestDescription,
                RequestDate = DateTime.UtcNow,
                RequestStatus = "Pending"
            };

            var created = await _repository.CreateAsync(request);
            _logger.LogInformation("Asset request (DTO) created by employee ID {EmployeeId}", employeeId);
            return Ok(created);
        }

        // Employee: View their own asset requests
        [Authorize(Roles = UserRoles.Employee)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRequests()
        {
            _logger.LogInformation("Employee requested their own asset requests.");

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Invalid or missing UserId claim.");
                return Unauthorized("UserId claim missing or invalid.");
            }

            var data = await _repository.GetByEmployeeIdAsync(employeeId);
            var result = data.Select(r => new AssetRequestResponseDto
            {
                RequestId = r.RequestId,
                AssetCategory = r.AssetCategory,
                RequestDescription = r.RequestDescription,
                RequestStatus = r.RequestStatus,
                RequestDate = r.RequestDate,
                EmployeeName = $"{r.Employee?.FirstName} {r.Employee?.LastName}",
                RejectionReason = r.RejectionReason
            });

            return Ok(result);
        }


        // Admin: Approve a request
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(long id)
        {
            _logger.LogInformation("Attempting to approve asset request ID {RequestId}", id);

            var adminIdClaim = User.FindFirst("UserId");
            if (adminIdClaim == null)
            {
                _logger.LogWarning("Admin UserId claim not found for approval.");
                return Unauthorized("User ID not found in token.");
            }

            var result = await _repository.ApproveAsync(id, long.Parse(adminIdClaim.Value));

            if (result)
            {
                _logger.LogInformation("Asset request ID {RequestId} approved by admin.", id);
                return Ok("Request approved successfully.");
            }

            _logger.LogWarning("Asset request ID {RequestId} not found for approval.", id);
            return NotFound("Request not found.");
        }

        // Admin: Reject a request
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(long id, [FromQuery] string reason)
        {
            _logger.LogInformation("Attempting to reject asset request ID {RequestId} with reason: {Reason}", id, reason);

            var result = await _repository.RejectAsync(id, reason);

            if (result)
            {
                _logger.LogInformation("Asset request ID {RequestId} rejected.", id);
                return Ok("Request rejected.");
            }

            _logger.LogWarning("Asset request ID {RequestId} not found for rejection.", id);
            return NotFound("Request not found.");
        }
    }
}

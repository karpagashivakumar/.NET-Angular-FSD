using AssetDAL.DataAccess;
using AssetDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetServiceLayer.Helpers;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AssetServiceLayer.DTO;
using Microsoft.Extensions.Logging;

namespace AssetServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestRepository _repository;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(IServiceRequestRepository repository, ILogger<ServiceRequestController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Admin: Get all service requests
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger.LogInformation("Admin is fetching all service requests.");

            var requests = await _repository.GetAllAsync();

            var result = requests.Select(r => new ServiceRequestResponseDto
            {
                ServiceRequestId = r.ServiceRequestId,
                AssetNo = r.AssetNo,
                Description = r.Description,
                IssueType = r.IssueType,
                ServiceStatus = r.ServiceStatus,
                EmployeeName = $"{r.Employee?.FirstName} {r.Employee?.LastName}",
                ResolutionNotes = r.ResolutionNotes
            });

            return Ok(result);
        }

        // Admin: Get open service requests
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            _logger.LogInformation("Admin is fetching open service requests.");
            var data = await _repository.GetOpenAsync();
            return Ok(data);
        }

        // Admin: Get service requests by status
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            _logger.LogInformation("Admin is fetching service requests with status: {Status}", status);
            var data = await _repository.GetByStatusAsync(status);
            return Ok(data);
        }

        // Employee: Create new service request (directly)
        [Authorize(Roles = UserRoles.Employee)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid for service request.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Employee claim missing or invalid during service request creation.");
                return Unauthorized("UserId claim missing or invalid.");
            }

            request.EmployeeId = employeeId;

            var created = await _repository.CreateAsync(request);
            _logger.LogInformation("Service request created by employee ID {EmployeeId}", employeeId);
            return Ok(created);
        }

        // Employee: Create with DTO
        [Authorize(Roles = UserRoles.Employee)]
        [HttpPost("with-dto")]
        public async Task<IActionResult> Create([FromBody] ServiceRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model for service request DTO.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Employee claim missing or invalid for DTO-based service request.");
                return Unauthorized("UserId claim missing or invalid.");
            }

            var request = new ServiceRequest
            {
                AssetId = dto.AssetId,
                AssetNo = dto.AssetNo,
                Description = dto.Description,
                IssueType = dto.IssueType,
                EmployeeId = employeeId,
                RequestDate = DateTime.UtcNow,
                ServiceStatus = "Open"
            };

            var created = await _repository.CreateAsync(request);
            _logger.LogInformation("DTO-based service request created by employee ID {EmployeeId}", employeeId);
            return Ok(created);
        }

        // Admin: Assign request
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/assign")]
        public async Task<IActionResult> Assign(long id, [FromQuery] long assignedTo)
        {
            _logger.LogInformation("Assigning service request ID {RequestId} to employee ID {EmployeeId}", id, assignedTo);

            var result = await _repository.AssignAsync(id, assignedTo);
            if (result)
            {
                _logger.LogInformation("Service request ID {RequestId} assigned successfully.", id);
                return Ok("Service request assigned.");
            }

            _logger.LogWarning("Failed to assign: service request ID {RequestId} not found.", id);
            return NotFound("Request not found.");
        }

        // Admin: Resolve request
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> Resolve(long id, [FromQuery] string notes)
        {
            _logger.LogInformation("Resolving service request ID {RequestId} with notes.", id);

            var result = await _repository.ResolveAsync(id, notes);
            if (result)
            {
                _logger.LogInformation("Service request ID {RequestId} resolved.", id);
                return Ok("Service request resolved.");
            }

            _logger.LogWarning("Failed to resolve: service request ID {RequestId} not found.", id);
            return NotFound("Request not found.");
        }

        // Admin: Close request
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(long id)
        {
            _logger.LogInformation("Closing service request ID {RequestId}.", id);

            var result = await _repository.CloseAsync(id);
            if (result)
            {
                _logger.LogInformation("Service request ID {RequestId} closed successfully.", id);
                return Ok("Service request closed.");
            }

            _logger.LogWarning("Failed to close: service request ID {RequestId} not found.", id);
            return NotFound("Request not found.");
        }

        // Employee: Get their own requests
        [Authorize(Roles = UserRoles.Employee)]
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Failed to retrieve my requests: UserId claim missing or invalid.");
                return Unauthorized("UserId claim is missing or invalid.");
            }

            var allRequests = await _repository.GetAllAsync();
            var requests = await _repository.GetAllAsync();

            var myRequest = requests.Select(r => new ServiceRequestResponseDto
            {
                ServiceRequestId = r.ServiceRequestId,
                AssetNo = r.AssetNo,
                Description = r.Description,
                IssueType = r.IssueType,
                ServiceStatus = r.ServiceStatus,
                EmployeeName = $"{r.Employee?.FirstName} {r.Employee?.LastName}",
                ResolutionNotes = r.ResolutionNotes
            });

            _logger.LogInformation("Employee ID {EmployeeId} fetched their own service requests.", employeeId);
            return Ok(myRequest);
        }
    }
}

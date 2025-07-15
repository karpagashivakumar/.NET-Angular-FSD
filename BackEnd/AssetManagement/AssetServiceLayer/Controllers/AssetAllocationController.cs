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
    public class AssetAllocationController : ControllerBase
    {
        private readonly IAssetAllocationRepository _repository;
        private readonly ILogger<AssetAllocationController> _logger;

        public AssetAllocationController(IAssetAllocationRepository repository, ILogger<AssetAllocationController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Admin: View all allocations
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Admin requested all asset allocations.");

            var allocations = await _repository.GetAllAsync();

            var result = allocations.Select(a => new AssetAllocationResponseDto
            {
                AllocationId = a.AllocationId,
                AssetName = a.Asset?.AssetName,
                EmployeeName = $"{a.Employee?.FirstName} {a.Employee?.LastName}",
                AllocatedDate = a.AllocatedDate,
                ReturnDate = a.ReturnDate,
                Status = a.AllocationStatus,
                Remarks = a.Remarks
            });

            return Ok(result);
        }

        // Employee: View their own allocations
        [Authorize(Roles = UserRoles.Employee)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAllocations()
        {
            _logger.LogInformation("Employee is fetching their own allocations.");

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found in JWT for GetMyAllocations.");
                return Unauthorized("User ID not found in token.");
            }

            long employeeId = long.Parse(userIdClaim.Value);
            var data = await _repository.GetByEmployeeAsync(employeeId);

            _logger.LogInformation("Allocations retrieved for employee ID {EmployeeId}", employeeId);
            return Ok(data);
        }

        // Admin: Create asset allocation (raw entity)
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetAllocation allocation)
        {
            _logger.LogInformation("Creating asset allocation via raw model.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating allocation (raw).");
                return BadRequest(ModelState);
            }

            var adminIdClaim = User.FindFirst("UserId");
            if (adminIdClaim == null)
            {
                _logger.LogWarning("UserId claim missing while creating allocation.");
                return Unauthorized("User ID not found in token.");
            }

            allocation.AllocatedBy = long.Parse(adminIdClaim.Value);

            var created = await _repository.CreateAsync(allocation);
            _logger.LogInformation("Asset allocated. Allocation ID: {AllocationId}", created.AllocationId);
            return Ok(created);
        }

        // Admin: Create asset allocation (DTO)
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("with-dto")]
        public async Task<IActionResult> Create([FromBody] AssetAllocationRequest dto)
        {
            _logger.LogInformation("Creating asset allocation via DTO.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating allocation (DTO).");
                return BadRequest(ModelState);
            }

            var allocation = new AssetAllocation
            {
                AssetId = dto.AssetId,
                EmployeeId = dto.EmployeeId,
                AllocatedBy = dto.AllocatedBy,
                Remarks = dto.Remarks,
                AllocationStatus = "Active",
                AllocatedDate = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(allocation);
            _logger.LogInformation("Asset allocation (DTO) created with ID {AllocationId}", created.AllocationId);

            return Ok(created);
        }

        // Employee: Return allocated asset
        [Authorize(Roles = UserRoles.Employee)]
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnAsset(long id, [FromBody] ReturnAssetDto dto)
        {
            _logger.LogInformation("Employee is attempting to return asset allocation ID {AllocationId}", id);

            var result = await _repository.ReturnAssetAsync(id, dto.Remarks);
            if (result)
            {
                _logger.LogInformation("Asset allocation ID {AllocationId} returned successfully.", id);
                return Ok("Asset returned successfully.");
            }

            _logger.LogWarning("Asset allocation ID {AllocationId} not found for return.", id);
            return NotFound("Allocation not found.");
        }
    }
}

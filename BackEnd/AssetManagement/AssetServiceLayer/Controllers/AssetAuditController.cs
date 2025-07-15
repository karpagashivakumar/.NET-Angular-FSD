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
    public class AssetAuditController : ControllerBase
    {
        private readonly IAssetAuditRepository _repository;
        private readonly ILogger<AssetAuditController> _logger;

        public AssetAuditController(IAssetAuditRepository repository, ILogger<AssetAuditController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Admin: View all audits
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all asset audits.");

            var audits = await _repository.GetAllAsync();

            var result = audits.Select(a => new AssetAuditResponseDto
            {
                AuditId = a.AuditId,
                AssetName = a.Asset?.AssetName,
                EmployeeName = $"{a.Employee?.FirstName} {a.Employee?.LastName}",
                AuditStatus = a.AuditStatus,
                Notes = a.AuditNotes
            });

            return Ok(result);
        }

        // Employee: Get audits assigned to them
        [Authorize(Roles = UserRoles.Employee)]
        [HttpGet("my-audits")]
        public async Task<IActionResult> GetMyAudits()
        {
            _logger.LogInformation("Fetching audits for the logged-in employee.");

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long employeeId))
            {
                _logger.LogWarning("Invalid or missing UserId claim.");
                return Unauthorized("Invalid token.");
            }

            var audits = await _repository.GetByEmployeeIdAsync(employeeId);

            var result = audits.Select(a => new AssetAuditResponseDto
            {
                AuditId = a.AuditId,
                AssetName = a.Asset?.AssetName,
                EmployeeName = $"{a.Employee?.FirstName} {a.Employee?.LastName}",
                AuditStatus = a.AuditStatus,
                Notes = a.AuditNotes
            });

            return Ok(result);
        }


        // Admin: View pending audits
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            _logger.LogInformation("Fetching all pending audits.");
            var audits = await _repository.GetPendingAsync();
            return Ok(audits);
        }

        // Admin: Create new audit with RequestedBy from JWT
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetAudit audit)
        {
            _logger.LogInformation("Creating asset audit (raw entity).");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model while creating asset audit.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                _logger.LogWarning("Missing UserId claim for audit creation.");
                return Unauthorized("Missing user identity.");
            }

            audit.RequestedBy = long.Parse(userIdClaim.Value);

            var created = await _repository.CreateAsync(audit);
            _logger.LogInformation("Asset audit created with ID {AuditId}", created.AuditId);

            return Ok(created);
        }

        // Admin: Create audit via DTO
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("with-dto")]
        public async Task<IActionResult> Create([FromBody] AssetAuditDto dto)
        {
            _logger.LogInformation("Creating asset audit via DTO.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model while creating audit via DTO.");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            long.TryParse(userIdClaim, out long requestedBy);

            var audit = new AssetAudit
            {
                EmployeeId = dto.EmployeeId,
                AssetId = dto.AssetId,
                AuditNotes = dto.AuditNotes,
                AuditRequestDate = DateTime.UtcNow,
                AuditStatus = "Pending",
                RequestedBy = requestedBy > 0 ? requestedBy : null
            };

            var created = await _repository.CreateAsync(audit);
            _logger.LogInformation("Asset audit (DTO) created with ID {AuditId}", created.AuditId);

            return Ok(created);
        }

        // Admin: Complete a pending audit
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> CompleteAudit(long id, [FromBody] string notes)
        {
            _logger.LogInformation("Completing audit ID {AuditId}", id);

            var result = await _repository.CompleteAsync(id, notes);
            if (result)
            {
                _logger.LogInformation("Audit ID {AuditId} marked as completed.", id);
                return Ok("Audit marked as completed.");
            }

            _logger.LogWarning("Audit ID {AuditId} not found to complete.", id);
            return NotFound("Audit not found.");
        }
    }
}

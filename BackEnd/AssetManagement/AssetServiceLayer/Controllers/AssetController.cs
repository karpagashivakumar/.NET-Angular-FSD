using AssetDAL.DataAccess;
using AssetDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetServiceLayer.Helpers;
using AssetServiceLayer.DTO;
using Microsoft.Extensions.Logging;

namespace AssetServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<AssetController> _logger;

        public AssetController(IAssetRepository assetRepository, ILogger<AssetController> logger)
        {
            _assetRepository = assetRepository;
            _logger = logger;
        }

        // GET: api/v1/Asset
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            _logger.LogInformation("Fetching all assets.");

            var assets = await _assetRepository.GetAllAsync();

            var result = assets.Select(a => new AssetSummaryDto
            {
                AssetId = a.AssetId,
                AssetNo = a.AssetNo,
                AssetName = a.AssetName,
                AssetCategory = a.AssetCategory,
                AssetStatus = a.AssetStatus,
                AssetValue = a.AssetValue
            });

            return Ok(result);
        }

        // GET: api/v1/Asset/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsset(long id)
        {
            _logger.LogInformation("Fetching asset with ID {AssetId}", id);

            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                _logger.LogWarning("Asset not found with ID {AssetId}", id);
                return NotFound();
            }

            return Ok(asset);
        }

        // GET: api/v1/Asset/category/{category}
        [Authorize]
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            _logger.LogInformation("Fetching assets by category: {Category}", category);

            var assets = await _assetRepository.GetByCategoryAsync(category);
            return Ok(assets);
        }

        // POST: api/v1/Asset
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] AssetCreateDto dto)
        {
            _logger.LogInformation("Creating new asset: {AssetNo}", dto.AssetNo);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating asset.");
                return BadRequest(ModelState);
            }

            var asset = new Asset
            {
                AssetNo = dto.AssetNo,
                AssetName = dto.AssetName,
                AssetCategory = dto.AssetCategory,
                AssetModel = dto.AssetModel,
                ManufacturingDate = dto.ManufacturingDate,
                ExpiryDate = dto.ExpiryDate,
                AssetValue = dto.AssetValue,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CreatedDate = DateTime.UtcNow,
                AssetStatus = "Available"
            };

            var created = await _assetRepository.CreateAsync(asset);
            _logger.LogInformation("Asset created successfully with ID {AssetId}", created.AssetId);

            return Ok(created);
        }

        // PUT: api/v1/Asset/{id}
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(long id, [FromBody] AssetUpdateDto dto)
        {
            _logger.LogInformation("Updating asset with ID {AssetId}", id);

            if (id != dto.AssetId)
            {
                _logger.LogWarning("Asset ID mismatch: Route ID = {RouteId}, Body ID = {BodyId}", id, dto.AssetId);
                return BadRequest("Asset ID mismatch.");
            }

            var existing = await _assetRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Asset not found with ID {AssetId}", id);
                return NotFound("Asset not found.");
            }

            existing.AssetNo = dto.AssetNo;
            existing.AssetName = dto.AssetName;
            existing.AssetCategory = dto.AssetCategory;
            existing.AssetModel = dto.AssetModel;
            existing.ManufacturingDate = dto.ManufacturingDate;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.AssetValue = dto.AssetValue;
            existing.Description = dto.Description;
            existing.ImageUrl = dto.ImageUrl;
            existing.AssetStatus = dto.AssetStatus;
            existing.UpdatedDate = DateTime.UtcNow;

            var updated = await _assetRepository.UpdateAsync(existing);
            _logger.LogInformation("Asset ID {AssetId} updated successfully.", id);

            return Ok(updated);
        }

        // DELETE: api/v1/Asset/{id}
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(long id)
        {
            _logger.LogInformation("Deleting asset with ID {AssetId}", id);

            var deleted = await _assetRepository.DeleteAsync(id);
            if (deleted)
            {
                _logger.LogInformation("Asset ID {AssetId} deleted successfully.", id);
                return Ok("Asset deleted successfully.");
            }

            _logger.LogWarning("Asset not found with ID {AssetId}", id);
            return NotFound("Asset not found.");
        }
    }
}

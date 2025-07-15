using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssetDAL.DataAccess
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AssetManagementDbContext _context;

        public AssetRepository(AssetManagementDbContext context) => _context = context;

        public async Task<Asset?> GetByIdAsync(long assetId) => await _context.Assets.FindAsync(assetId);

        public async Task<IEnumerable<Asset>> GetAllAsync() => await _context.Assets.ToListAsync();

        public async Task<IEnumerable<Asset>> GetByCategoryAsync(string category)
            => await _context.Assets.Where(a => a.AssetCategory == category).ToListAsync();

        public async Task<Asset> CreateAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<bool> DeleteAsync(long assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null) return false;
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

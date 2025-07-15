using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(long assetId);
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<IEnumerable<Asset>> GetByCategoryAsync(string category);
        Task<Asset> CreateAsync(Asset asset);
        Task<Asset> UpdateAsync(Asset asset);
        Task<bool> DeleteAsync(long assetId);
    }
}

using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IAssetAllocationRepository
    {
        Task<AssetAllocation> CreateAsync(AssetAllocation allocation);
        Task<IEnumerable<AssetAllocation>> GetAllAsync();
        Task<IEnumerable<AssetAllocation>> GetByEmployeeAsync(long employeeId);
        Task<bool> ReturnAssetAsync(long allocationId, string remarks);
    }
}

using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssetDAL.DataAccess
{
    public class AssetAllocationRepository : IAssetAllocationRepository
    {
        private readonly AssetManagementDbContext _context;

        public AssetAllocationRepository(AssetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AssetAllocation> CreateAsync(AssetAllocation allocation)
        {
            _context.AssetAllocations.Add(allocation);
            await _context.SaveChangesAsync();
            return allocation;
        }

        public async Task<IEnumerable<AssetAllocation>> GetAllAsync()
        {
            return await _context.AssetAllocations
        .Include(a => a.Asset)
        .Include(a => a.Employee)
        .ToListAsync();
        }

        public async Task<IEnumerable<AssetAllocation>> GetByEmployeeAsync(long employeeId)
        {
            return await _context.AssetAllocations
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> ReturnAssetAsync(long allocationId, string remarks)
        {
            var allocation = await _context.AssetAllocations.FindAsync(allocationId);
            if (allocation == null) return false;

            allocation.AllocationStatus = "Returned";
            allocation.ReturnDate = DateTime.UtcNow;
            allocation.Remarks = remarks;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

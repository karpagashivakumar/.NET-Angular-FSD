using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssetDAL.DataAccess
{
    public class AssetRequestRepository : IAssetRequestRepository
    {
        private readonly AssetManagementDbContext _context;

        public AssetRequestRepository(AssetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AssetRequest> CreateAsync(AssetRequest request)
        {
            _context.AssetRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<AssetRequest>> GetAllAsync()
        {
            return await _context.AssetRequests
                .Include(r => r.Employee) 
                .ToListAsync();
        }


        public async Task<IEnumerable<AssetRequest>> GetPendingAsync()
        {
            return await _context.AssetRequests
                .Where(r => r.RequestStatus == "Pending")
                .ToListAsync();
        }

        public async Task<bool> ApproveAsync(long requestId, long approvedBy)
        {
            var request = await _context.AssetRequests.FindAsync(requestId);
            if (request == null) return false;

            request.RequestStatus = "Approved";
            request.ApprovedBy = approvedBy;
            request.ApprovedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AssetRequest>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _context.AssetRequests
                .Where(r => r.EmployeeId == employeeId)
                .Include(r => r.Employee)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }


        public async Task<bool> RejectAsync(long requestId, string reason)
        {
            var request = await _context.AssetRequests.FindAsync(requestId);
            if (request == null) return false;

            request.RequestStatus = "Rejected";
            request.RejectionReason = reason;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

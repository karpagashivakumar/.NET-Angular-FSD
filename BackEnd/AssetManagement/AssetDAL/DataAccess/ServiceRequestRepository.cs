using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssetDAL.DataAccess
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AssetManagementDbContext _context;

        public ServiceRequestRepository(AssetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRequest> CreateAsync(ServiceRequest request)
        {
            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequests
                .Include(r => r.Asset)
                .Include(r => r.Employee)
                .Include(r => r.AssignedToUser)
                .ToListAsync();
        }


        public async Task<IEnumerable<ServiceRequest>> GetOpenAsync()
        {
            return await _context.ServiceRequests
                .Where(s => s.ServiceStatus == "Open")
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequest>> GetByStatusAsync(string status)
        {
            return await _context.ServiceRequests
                .Where(s => s.ServiceStatus == status)
                .ToListAsync();
        }

        public async Task<bool> AssignAsync(long requestId, long assignedTo)
        {
            var request = await _context.ServiceRequests.FindAsync(requestId);
            if (request == null) return false;

            request.AssignedTo = assignedTo;
            request.ServiceStatus = "InProgress";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResolveAsync(long requestId, string notes)
        {
            var request = await _context.ServiceRequests.FindAsync(requestId);
            if (request == null) return false;

            request.ServiceStatus = "Resolved";
            request.ResolutionNotes = notes;
            request.ResolutionDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseAsync(long requestId)
        {
            var request = await _context.ServiceRequests.FindAsync(requestId);
            if (request == null) return false;

            request.ServiceStatus = "Closed";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssetDAL.DataAccess
{
    public class AssetAuditRepository : IAssetAuditRepository
    {
        private readonly AssetManagementDbContext _context;

        public AssetAuditRepository(AssetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AssetAudit> CreateAsync(AssetAudit audit)
        {
            _context.AssetAudits.Add(audit);
            await _context.SaveChangesAsync();
            return audit;
        }

        public async Task<IEnumerable<AssetAudit>> GetAllAsync()
        {
            return await _context.AssetAudits
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetAudit>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _context.AssetAudits
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }


        public async Task<IEnumerable<AssetAudit>> GetPendingAsync()
        {
            return await _context.AssetAudits
                .Where(a => a.AuditStatus == "Pending")
                .ToListAsync();
        }

        public async Task<bool> CompleteAsync(long auditId, string notes)
        {
            var audit = await _context.AssetAudits.FindAsync(auditId);
            if (audit == null) return false;

            audit.AuditStatus = "Completed";
            audit.AuditResponseDate = DateTime.UtcNow;
            audit.AuditNotes = notes;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

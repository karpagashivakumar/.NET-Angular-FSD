using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IAssetAuditRepository
    {
        Task<AssetAudit> CreateAsync(AssetAudit audit);
        Task<IEnumerable<AssetAudit>> GetAllAsync();
        Task<IEnumerable<AssetAudit>> GetPendingAsync();
        Task<IEnumerable<AssetAudit>> GetByEmployeeIdAsync(long employeeId);

        Task<bool> CompleteAsync(long auditId, string notes);
    }
}

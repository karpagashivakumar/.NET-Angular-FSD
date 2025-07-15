using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IAssetRequestRepository
    {
        Task<AssetRequest> CreateAsync(AssetRequest request);
        Task<IEnumerable<AssetRequest>> GetAllAsync();
        Task<IEnumerable<AssetRequest>> GetPendingAsync();
        Task<bool> ApproveAsync(long requestId, long approvedBy);
        Task<bool> RejectAsync(long requestId, string reason);
        Task<IEnumerable<AssetRequest>> GetByEmployeeIdAsync(long employeeId);

    }
}

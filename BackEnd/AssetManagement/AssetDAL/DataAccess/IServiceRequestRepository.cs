using AssetDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IServiceRequestRepository
    {
        Task<ServiceRequest> CreateAsync(ServiceRequest request);
        Task<IEnumerable<ServiceRequest>> GetAllAsync();
        Task<IEnumerable<ServiceRequest>> GetOpenAsync();
        Task<IEnumerable<ServiceRequest>> GetByStatusAsync(string status);
        Task<bool> AssignAsync(long requestId, long assignedTo);
        Task<bool> ResolveAsync(long requestId, string notes);
        Task<bool> CloseAsync(long requestId);
    }
}

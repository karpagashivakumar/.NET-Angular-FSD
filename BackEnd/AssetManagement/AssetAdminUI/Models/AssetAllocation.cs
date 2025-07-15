using System;
using System.ComponentModel.DataAnnotations;

namespace AssetAdminUI.Models
{
    public class AssetAllocation
    {
        public long AllocationId { get; set; }

        [Required]
        public long AssetId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        public long AllocatedBy { get; set; }

        public string? Remarks { get; set; }

        public string? AllocationStatus { get; set; }

        public DateTime AllocatedDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Display purposes only
        public string? AssetName { get; set; }
        public string? EmployeeName { get; set; }
    }
}


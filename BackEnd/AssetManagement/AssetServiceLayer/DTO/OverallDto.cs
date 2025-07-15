using System.Globalization;

namespace AssetServiceLayer.DTO
{
    public class UserDto
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AssetSummaryDto
    {
        public long AssetId { get; set; }
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public string AssetCategory { get; set; }
        public string AssetStatus { get; set; }
        public decimal? AssetValue { get; set; }
    }

    public class AssetAllocationResponseDto
    {
        public long AllocationId { get; set; }
        public string AssetName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AllocatedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class ServiceRequestResponseDto
    {
        public long ServiceRequestId { get; set; }
        public string AssetNo { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public string ServiceStatus { get; set; }
        public string EmployeeName { get; set; }
        public string? ResolutionNotes { get; set; }
    }

    public class AssetAuditResponseDto
    {
        public long AuditId { get; set; }
        public string AssetName { get; set; }
        public string EmployeeName { get; set; }
        public string AuditStatus { get; set; }
        public string? Notes { get; set; }
    }
    public class AssetRequestResponseDto
    {
        public long RequestId { get; set; }
        public string AssetCategory { get; set; }
        public string RequestDescription { get; set; }
        public string RequestStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public string EmployeeName { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Only for creation
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }

    public class UserUpdateDto
    {
        public long UserId { get; set; }
        public string Username { get; set; } 
        public string Email { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        //public string Role { get; set; }
        //public bool IsActive { get; set; }
    }

    public class AssetCreateDto
    {
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public string AssetCategory { get; set; }
        public string? AssetModel { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? AssetValue { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class AssetUpdateDto : AssetCreateDto
    {
        public long AssetId { get; set; }
        public string AssetStatus { get; set; }
    }

    public class ReturnAssetDto
    {
        public string Remarks { get; set; }
    }
}

namespace AssetServiceLayer.DTO
{
    public class AssetAllocationRequest
    {
        public long AssetId { get; set; }
        public long EmployeeId { get; set; }
        public long? AllocatedBy { get; set; }
        public string? Remarks { get; set; }
    }

}

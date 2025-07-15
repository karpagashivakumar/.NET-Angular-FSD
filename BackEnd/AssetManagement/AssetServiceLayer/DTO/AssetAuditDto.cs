namespace AssetServiceLayer.DTO
{
    public class AssetAuditDto
    {
        public long EmployeeId { get; set; }
        public long AssetId { get; set; }
        public string? AuditNotes { get; set; }
    }
}


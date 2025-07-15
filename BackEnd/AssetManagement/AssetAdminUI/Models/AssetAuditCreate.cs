namespace AssetAdminUI.Models
{
    public class AssetAuditCreate
    {
        public long EmployeeId { get; set; }
        public long AssetId { get; set; }
        public string AuditNotes { get; set; }
    }

    public class AssetAuditComplete
    {
        public long AuditId { get; set; }
        public string Notes { get; set; }
    }
}

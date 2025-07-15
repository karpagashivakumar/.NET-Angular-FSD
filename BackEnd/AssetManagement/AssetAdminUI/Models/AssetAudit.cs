namespace AssetAdminUI.Models
{
    public class AssetAudit
    {
        public long AuditId { get; set; }
        public long EmployeeId { get; set; }
        public long AssetId { get; set; }
        public string AuditNotes { get; set; }
        public string AuditStatus { get; set; }
        public string AssetName { get; set; }
        public string EmployeeName { get; set; }
    }
}

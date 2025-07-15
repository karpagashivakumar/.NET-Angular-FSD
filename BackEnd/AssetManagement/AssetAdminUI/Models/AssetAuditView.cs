namespace AssetAdminUI.Models
{
    public class AssetAuditView
    {
        public long AuditId { get; set; }
        public string AssetName { get; set; }
        public string EmployeeName { get; set; }
        public string AuditStatus { get; set; }
        public string Notes { get; set; }
    }
}

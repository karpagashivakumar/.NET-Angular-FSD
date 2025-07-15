namespace AssetAdminUI.Models
{
    public class AssetRequestView
    {
        public long RequestId { get; set; }
        public string AssetCategory { get; set; }
        public string RequestDescription { get; set; }
        public string RequestStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public string EmployeeName { get; set; }
        public string? RejectionReason { get; set; }
    }
}

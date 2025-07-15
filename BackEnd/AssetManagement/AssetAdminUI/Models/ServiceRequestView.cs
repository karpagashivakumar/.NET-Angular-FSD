namespace AssetAdminUI.Models
{
    public class ServiceRequestView
    {
        public long ServiceRequestId { get; set; }
        public string AssetNo { get; set; }
        public string Description { get; set; }
        public string IssueType { get; set; }
        public string ServiceStatus { get; set; }
        public string EmployeeName { get; set; }
        public string ResolutionNotes { get; set; }
    }

    public class ServiceRequestAssign
    {
        public long ServiceRequestId { get; set; }
        public long AssignedTo { get; set; }
    }

    public class ServiceRequestResolve
    {
        public long ServiceRequestId { get; set; }
        public string Notes { get; set; }
    }
}

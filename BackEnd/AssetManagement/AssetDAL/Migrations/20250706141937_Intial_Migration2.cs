using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetDAL.Migrations
{
    /// <inheritdoc />
    public partial class Intial_Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Gender",
                table: "Users",
                sql: "Gender IN ('Male', 'Female', 'Other')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Role",
                table: "Users",
                sql: "Role IN ('Admin', 'Manager', 'Employee')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ServiceRequest_IssueType",
                table: "ServiceRequests",
                sql: "IssueType IN ('Repair', 'Maintenance', 'Replacement', 'Upgrade', 'Other')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ServiceRequest_ResolutionDate",
                table: "ServiceRequests",
                sql: "ResolutionDate IS NULL OR ResolutionDate >= RequestDate");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ServiceRequest_Status",
                table: "ServiceRequests",
                sql: "ServiceStatus IN ('Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Asset_ManufacturingDate",
                table: "Assets",
                sql: "ManufacturingDate <= GETDATE()");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Asset_Status",
                table: "Assets",
                sql: "AssetStatus IN ('Available', 'Allocated', 'UnderMaintenance', 'Disposed', 'Lost')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetRequest_ApprovedDate",
                table: "AssetRequests",
                sql: "ApprovedDate IS NULL OR ApprovedDate >= RequestDate");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetRequest_Status",
                table: "AssetRequests",
                sql: "RequestStatus IN ('Pending', 'Approved', 'Rejected', 'Allocated')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetAudit_ResponseDate",
                table: "AssetAudits",
                sql: "AuditResponseDate IS NULL OR AuditResponseDate >= AuditRequestDate");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetAudit_Status",
                table: "AssetAudits",
                sql: "AuditStatus IN ('Pending', 'InProgress', 'Completed', 'Cancelled')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetAllocation_ReturnDate",
                table: "AssetAllocations",
                sql: "ReturnDate IS NULL OR ReturnDate >= AllocatedDate");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AssetAllocation_Status",
                table: "AssetAllocations",
                sql: "AllocationStatus IN ('Active', 'Returned', 'Transferred')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Gender",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Role",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ServiceRequest_IssueType",
                table: "ServiceRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ServiceRequest_ResolutionDate",
                table: "ServiceRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ServiceRequest_Status",
                table: "ServiceRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Asset_ManufacturingDate",
                table: "Assets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Asset_Status",
                table: "Assets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetRequest_ApprovedDate",
                table: "AssetRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetRequest_Status",
                table: "AssetRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetAudit_ResponseDate",
                table: "AssetAudits");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetAudit_Status",
                table: "AssetAudits");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetAllocation_ReturnDate",
                table: "AssetAllocations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AssetAllocation_Status",
                table: "AssetAllocations");
        }
    }
}

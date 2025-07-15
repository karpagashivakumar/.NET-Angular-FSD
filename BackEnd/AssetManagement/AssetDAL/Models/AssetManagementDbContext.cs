using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    public class AssetManagementDbContext : DbContext
    {
        public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options) : base(options)
        {
        }
        public AssetManagementDbContext() : base()
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAllocation> AssetAllocations { get; set; }
        public DbSet<AssetRequest> AssetRequests { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<AssetAudit> AssetAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Gender)
                    .HasMaxLength(10);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(15);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Asset entity
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.AssetId);
                entity.HasIndex(e => e.AssetNo).IsUnique();

                entity.Property(e => e.AssetNo)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.AssetName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.AssetCategory)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.AssetModel)
                    .HasMaxLength(100);

                entity.Property(e => e.AssetValue)
                    .HasPrecision(18, 2);

                entity.Property(e => e.AssetStatus)
                    .HasMaxLength(30)
                    .HasDefaultValue("Available");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure AssetAllocation entity
            modelBuilder.Entity<AssetAllocation>(entity =>
            {
                entity.HasKey(e => e.AllocationId);

                entity.Property(e => e.AllocatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.AllocationStatus)
                    .HasMaxLength(30)
                    .HasDefaultValue("Active");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500);

                // Configure relationships
                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.AssetAllocations)
                    .HasForeignKey(e => e.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Employee)
                    .WithMany(u => u.AssetAllocations)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AllocatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.AllocatedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure AssetRequest entity
            modelBuilder.Entity<AssetRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.AssetCategory)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.RequestDescription)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.RequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.RequestStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Property(e => e.RejectionReason)
                    .HasMaxLength(500);

                // Configure relationships
                entity.HasOne(e => e.Employee)
                    .WithMany(u => u.AssetRequests)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure ServiceRequest entity
            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.ServiceRequestId);

                entity.Property(e => e.AssetNo)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.IssueType)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(e => e.RequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.ServiceStatus)
                    .HasMaxLength(30)
                    .HasDefaultValue("Open");

                entity.Property(e => e.ResolutionNotes)
                    .HasMaxLength(1000);

                // Configure relationships
                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.ServiceRequests)
                    .HasForeignKey(e => e.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Employee)
                    .WithMany(u => u.ServiceRequests)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AssignedToUser)
                    .WithMany()
                    .HasForeignKey(e => e.AssignedTo)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure AssetAudit entity
            modelBuilder.Entity<AssetAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId);

                entity.Property(e => e.AuditRequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.AuditStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Property(e => e.AuditNotes)
                    .HasMaxLength(1000);

                // Configure relationships
                entity.HasOne(e => e.Employee)
                    .WithMany(u => u.AssetAudits)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.AssetAudits)
                    .HasForeignKey(e => e.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.RequestedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Add any additional constraints or configurations
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.Sqlite")
            {
                ConfigureAdditionalConstraints(modelBuilder);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure your connection string here
                optionsBuilder.UseSqlServer("Server=localhost;Database=AssetMS;Trusted_Connection=true;TrustServerCertificate=true;");
            }
        }

        private void ConfigureAdditionalConstraints(ModelBuilder modelBuilder)
        {
            // Add check constraints for enum-like fields
            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Gender", "Gender IN ('Male', 'Female', 'Other')");

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Role", "Role IN ('Admin', 'Manager', 'Employee')");

            modelBuilder.Entity<Asset>()
                .HasCheckConstraint("CK_Asset_Status", "AssetStatus IN ('Available', 'Allocated', 'UnderMaintenance', 'Disposed', 'Lost')");

            modelBuilder.Entity<AssetAllocation>()
                .HasCheckConstraint("CK_AssetAllocation_Status", "AllocationStatus IN ('Active', 'Returned', 'Transferred')");

            modelBuilder.Entity<AssetRequest>()
                .HasCheckConstraint("CK_AssetRequest_Status", "RequestStatus IN ('Pending', 'Approved', 'Rejected', 'Allocated')");

            modelBuilder.Entity<ServiceRequest>()
                .HasCheckConstraint("CK_ServiceRequest_Status", "ServiceStatus IN ('Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled')");

            modelBuilder.Entity<ServiceRequest>()
                .HasCheckConstraint("CK_ServiceRequest_IssueType", "IssueType IN ('Repair', 'Maintenance', 'Replacement', 'Upgrade', 'Other')");

            modelBuilder.Entity<AssetAudit>()
                .HasCheckConstraint("CK_AssetAudit_Status", "AuditStatus IN ('Pending', 'InProgress', 'Completed', 'Cancelled')");

            // Add date constraints
            modelBuilder.Entity<Asset>()
                .HasCheckConstraint("CK_Asset_ManufacturingDate", "ManufacturingDate <= GETDATE()");

            modelBuilder.Entity<AssetAllocation>()
                .HasCheckConstraint("CK_AssetAllocation_ReturnDate", "ReturnDate IS NULL OR ReturnDate >= AllocatedDate");

            modelBuilder.Entity<AssetRequest>()
                .HasCheckConstraint("CK_AssetRequest_ApprovedDate", "ApprovedDate IS NULL OR ApprovedDate >= RequestDate");

            modelBuilder.Entity<ServiceRequest>()
                .HasCheckConstraint("CK_ServiceRequest_ResolutionDate", "ResolutionDate IS NULL OR ResolutionDate >= RequestDate");

            modelBuilder.Entity<AssetAudit>()
                .HasCheckConstraint("CK_AssetAudit_ResponseDate", "AuditResponseDate IS NULL OR AuditResponseDate >= AuditRequestDate");
        }

        // Override SaveChanges to handle audit fields
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Asset asset && entry.State == EntityState.Modified)
                {
                    asset.UpdatedDate = DateTime.UtcNow;
                }

                // You can add more audit field updates here for other entities if needed
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    [Table("ServiceRequests")]
    public class ServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ServiceRequestId")]
        public long ServiceRequestId { get; set; }

        [Required]
        [Column("AssetId")]
        [ForeignKey("Asset")]
        public long AssetId { get; set; }

        [Required]
        [Column("EmployeeId")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("AssetNo", TypeName = "varchar(50)")]
        public string AssetNo { get; set; }

        [Required]
        [StringLength(1000)]
        [Column("Description", TypeName = "varchar(1000)")]
        public string Description { get; set; }

        [Required]
        [StringLength(30)]
        [Column("IssueType", TypeName = "varchar(30)")]
        public string IssueType { get; set; }

        [Required]
        [Column("RequestDate")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(30)]
        [Column("ServiceStatus", TypeName = "varchar(30)")]
        public string ServiceStatus { get; set; } = "Open";

        [Column("AssignedTo")]
        [ForeignKey("AssignedToUser")]
        public long? AssignedTo { get; set; }

        [Column("ResolutionDate")]
        public DateTime? ResolutionDate { get; set; }

        [StringLength(1000)]
        [Column("ResolutionNotes", TypeName = "varchar(1000)")]
        public string? ResolutionNotes { get; set; }

        // Navigation Properties
        public virtual Asset Asset { get; set; }
        public virtual User Employee { get; set; }
        public virtual User? AssignedToUser { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    [Table("AssetAudits")]
    public class AssetAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuditId")]
        public long AuditId { get; set; }

        [Required]
        [Column("EmployeeId")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

        [Required]
        [Column("AssetId")]
        [ForeignKey("Asset")]
        public long AssetId { get; set; }

        [Required]
        [Column("AuditRequestDate")]
        public DateTime AuditRequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        [Column("AuditStatus", TypeName = "varchar(20)")]
        public string AuditStatus { get; set; } = "Pending";

        [Column("AuditResponseDate")]
        public DateTime? AuditResponseDate { get; set; }

        [StringLength(1000)]
        [Column("AuditNotes", TypeName = "varchar(1000)")]
        public string? AuditNotes { get; set; }

        [Column("RequestedBy")]
        [ForeignKey("RequestedByUser")]
        public long? RequestedBy { get; set; }

        // Navigation Properties
        public virtual User? Employee { get; set; }
        public virtual Asset? Asset { get; set; }

        public virtual User? RequestedByUser { get; set; }
    }
}

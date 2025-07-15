using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    [Table("AssetRequests")]
    public class AssetRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RequestId")]
        public long RequestId { get; set; }

        [Required]
        [Column("EmployeeId")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("AssetCategory", TypeName = "varchar(50)")]
        public string AssetCategory { get; set; }

        [Required]
        [StringLength(1000)]
        [Column("RequestDescription", TypeName = "varchar(1000)")]
        public string RequestDescription { get; set; }

        [Required]
        [Column("RequestDate")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        [Column("RequestStatus", TypeName = "varchar(20)")]
        public string RequestStatus { get; set; } = "Pending";

        [Column("ApprovedBy")]
        [ForeignKey("ApprovedByUser")]
        public long? ApprovedBy { get; set; }

        [Column("ApprovedDate")]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(500)]
        [Column("RejectionReason", TypeName = "varchar(500)")]
        public string? RejectionReason { get; set; }

        // Navigation Properties
        public virtual User Employee { get; set; }
        public virtual User? ApprovedByUser { get; set; }
    }
}

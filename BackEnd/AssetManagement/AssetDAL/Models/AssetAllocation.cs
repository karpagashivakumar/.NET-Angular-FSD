using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    [Table("AssetAllocations")]
    public class AssetAllocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AllocationId")]
        public long AllocationId { get; set; }

        [Required]
        [Column("AssetId")]
        [ForeignKey("Asset")]
        public long AssetId { get; set; }

        [Required]
        [Column("EmployeeId")]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }

        [Required]
        [Column("AllocatedDate")]
        public DateTime AllocatedDate { get; set; } = DateTime.UtcNow;

        [Column("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(30)]
        [Column("AllocationStatus", TypeName = "varchar(30)")]
        public string AllocationStatus { get; set; } = "Active";

        [StringLength(500)]
        [Column("Remarks", TypeName = "varchar(500)")]
        public string? Remarks { get; set; }

        [Column("AllocatedBy")]
        [ForeignKey("AllocatedByUser")]
        public long? AllocatedBy { get; set; }

        // Navigation Properties
        public virtual Asset Asset { get; set; }
        public virtual User Employee { get; set; }
        public virtual User? AllocatedByUser { get; set; }
    }
}

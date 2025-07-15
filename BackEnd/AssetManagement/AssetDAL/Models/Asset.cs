using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AssetDAL.Models
{
    [Table("Assets")]
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AssetId")]
        public long AssetId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("AssetNo", TypeName = "varchar(50)")]
        public string AssetNo { get; set; }

        [Required]
        [StringLength(100)]
        [Column("AssetName", TypeName = "varchar(100)")]
        public string AssetName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("AssetCategory", TypeName = "varchar(50)")]
        public string AssetCategory { get; set; }

        [StringLength(100)]
        [Column("AssetModel", TypeName = "varchar(100)")]
        public string? AssetModel { get; set; }

        [Column("ManufacturingDate")]
        public DateTime? ManufacturingDate { get; set; }

        [Column("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Column("AssetValue", TypeName = "decimal(18,2)")]
        public decimal? AssetValue { get; set; }

        [Required]
        [StringLength(30)]
        [Column("AssetStatus", TypeName = "varchar(30)")]
        public string AssetStatus { get; set; } = "Available";

        [StringLength(1000)]
        [Column("Description", TypeName = "varchar(1000)")]
        public string? Description { get; set; }

        [StringLength(500)]
        [Column("ImageUrl", TypeName = "varchar(500)")]
        public string? ImageUrl { get; set; }

        [Required]
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public virtual ICollection<AssetAllocation> AssetAllocations { get; set; } = new List<AssetAllocation>();

        [JsonIgnore]
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

        [JsonIgnore]
        public virtual ICollection<AssetAudit> AssetAudits { get; set; } = new List<AssetAudit>();
    }
}

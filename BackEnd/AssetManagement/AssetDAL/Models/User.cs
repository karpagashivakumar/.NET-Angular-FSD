using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")]
        public long UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Username", TypeName = "varchar(50)")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Password", TypeName = "varchar(255)")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Column("FirstName", TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("LastName", TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [StringLength(10)]
        [Column("Gender", TypeName = "varchar(10)")]
        public string? Gender { get; set; }

        [StringLength(15)]
        [Column("ContactNumber", TypeName = "varchar(15)")]
        public string? ContactNumber { get; set; }

        [StringLength(500)]
        [Column("Address", TypeName = "varchar(500)")]
        public string? Address { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Role", TypeName = "varchar(20)")]
        public string Role { get; set; }

        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("LastLoginDate")]
        public DateTime? LastLoginDate { get; set; }

        // Navigation Properties
        public virtual ICollection<AssetAllocation> AssetAllocations { get; set; } = new List<AssetAllocation>();
        public virtual ICollection<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
        public virtual ICollection<AssetAudit> AssetAudits { get; set; } = new List<AssetAudit>();
    }
}

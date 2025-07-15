using System;
using System.ComponentModel.DataAnnotations;

namespace AssetAdminUI.Models
{
    public class AssetEdit
    {
        [Required]
        public long AssetId { get; set; }

        [Required]
        public string AssetNo { get; set; }

        [Required]
        public string AssetName { get; set; }

        [Required]
        public string AssetCategory { get; set; }

        [Required]
        public string AssetModel { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public decimal AssetValue { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string AssetStatus { get; set; }

        public string? ImageUrl { get; set; }
    }
}


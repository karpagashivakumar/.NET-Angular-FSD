using System;
using System.ComponentModel.DataAnnotations;

namespace AssetAdminUI.Models
{
    public class Asset
    {
        public long AssetId { get; set; }

        [Required]
        public string AssetNo { get; set; }

        [Required]
        public string AssetName { get; set; }

        [Required]
        public string AssetCategory { get; set; }

        public string AssetModel { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public decimal AssetValue { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string AssetStatus { get; set; }
    }
}


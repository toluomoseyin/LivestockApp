using System;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class RestockLivestockViewModel
    {
        [Required]
        public string LivestockId { get; set; }
        [Required]
        public Decimal SellingPrice { get; set; }

        [Required]
        public Decimal PurchasePrice { get; set; }

        [Required]
        public Decimal Discount { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool Availability { get; set; }

    }
}

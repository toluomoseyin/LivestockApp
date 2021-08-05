using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class RestockLivestockDto
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

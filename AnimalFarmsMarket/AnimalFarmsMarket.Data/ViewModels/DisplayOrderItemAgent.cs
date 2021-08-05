using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class DisplayOrderItemAgent
    {
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal SellingPrice { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal Weight { get; set; }

        public byte Sex { get; set; }

        public string Breed { get; set; }

        public string Color { get; set; }

        public int Age { get; set; }
    }
}

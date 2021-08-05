using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivestockUpdateDto
    {
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal SellingPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Discount { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Weight { get; set; }

        [Required]
        public byte Sex { get; set; }

        [Required]
        [MaxLength(25)]
        public string Breed { get; set; }

        [Required]
        [MaxLength(25)]
        public string Color { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public bool Availability { get; set; } = true;

        [Required]
        public string MarketId { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string AgentId { get; set; }
    }
}

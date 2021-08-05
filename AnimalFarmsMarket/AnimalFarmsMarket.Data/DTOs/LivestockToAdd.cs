using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivestockToAdd
    {
        [MaxLength(250, ErrorMessage = "Maximum length alowed for Description field is 250 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Selling price is required")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Discount { get; set; }

        [Range(1, 200, ErrorMessage = "Quantity should be between 1 and 200")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Weight { get; set; }
        public byte Sex { get; set; }

        [Required(ErrorMessage = "Breed is required")]
        public string Breed { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Range(1,30,ErrorMessage = "Age should be between 1 and 30")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public bool Availability { get; set; }

        [Required(ErrorMessage = "Market field is required")]
        public string MarketId { get; set; }

        [Required(ErrorMessage = "Category field is required")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Agent field is required")]
        public string AgentId { get; set; }
    }
}

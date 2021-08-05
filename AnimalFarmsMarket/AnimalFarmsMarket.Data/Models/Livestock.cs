using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalFarmsMarket.Data.Models
{
    public class Livestock : BaseEntity
    {
        [Required]
        [MaxLength(250)]
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
        [MaxLength(10)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Weight { get; set; }

        [Required]
        [MaxLength(25)]
        public byte Sex { get; set; }

        [Required]
        [MaxLength(25)]
        public string Breed { get; set; }

        [Required]
        [MaxLength(25)]
        public string Color { get; set; }

        [Required]
        [MaxLength(10)]
        public int Age { get; set; }

        [Required]
        public bool Availability { get; set; } = true;

        public Market Market { get; set; }

        public string MarketId { get; set; }

        public Category Category { get; set; }

        public string CategoryId { get; set; }

        public Agent Agent { get; set; }

        public string AgentId { get; set; }

        public IEnumerable<Rating> Ratings { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<LivestockImages> Images { get; set; }

        public IEnumerable<OrderItems> OrderDetails { get; set; }

        public Livestock()
        {
            Ratings = new HashSet<Rating>();
            Reviews = new HashSet<Review>();
            Images = new HashSet<LivestockImages>();
            OrderDetails = new HashSet<OrderItems>();
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class MarketAddress : BaseEntity
    {
        public Market Market { get; set; }

        public string MarketId { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string Street { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string City { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string State { get; set; }
    }
}

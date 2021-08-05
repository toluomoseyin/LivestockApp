using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Address : BaseEntity
    {
        public AppUser User { get; set; }

        public string UserId { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string Street { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string City { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Street name cannot be more than 125")]
        public string State { get; set; }
        //public string Country { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Review : BaseEntity
    {
        public AppUser User { get; set; }

        public string UserId { get; set; }

        public Livestock Livestock { get; set; }

        public string LivestockId { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Review can not be more than 250 characters")]
        public string ReviewText { get; set; }
    }
}

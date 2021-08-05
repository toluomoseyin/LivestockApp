using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Testimony : BaseEntity
    {
        public AppUser User { get; set; }

        public string UserId { get; set; }

        [Required]
        [MaxLength(125, ErrorMessage = "Title cannot be more than 125")]
        public string Title { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Content cannot be more than 250")]
        public string Testimonies { get; set; }
    }
}

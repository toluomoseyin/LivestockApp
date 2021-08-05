using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ReviewToAddDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string LivestockId { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Review can not be more than 250 characters")]
        public string ReviewText { get; set; }
    }
}
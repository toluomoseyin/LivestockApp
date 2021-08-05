using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateReviewDto
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Review can not be more than 250 characters")]
        public string ReviewText { get; set; }
    }
}
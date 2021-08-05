using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    //original
    public class AddRatingDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string LivestockId { get; set; }
        [Required]
        public int RatingFigure { get; set; }
    }
}

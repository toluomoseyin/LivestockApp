using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateRatingDto
    {
        public string Id { get; set; }
        [Required]
        public int RatingFigure { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class GetRatingsDto
    {
        public string Id { get; set; }
        public int RatingFigure { get; set; }
        public string UserId { get; set; }
        public string LivestockId { get; set; }
    }
}

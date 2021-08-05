using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class SearchLivestockDto
    {
        public int PageNumber { get; set; } = 1;

        public string Type { get; set; }

        public byte Sex { get; set; } 

        public decimal Weight { get; set; }

        public string Breed { get; set; }

    }
}

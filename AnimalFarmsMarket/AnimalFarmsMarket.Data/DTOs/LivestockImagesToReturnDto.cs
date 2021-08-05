using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivestockImagesToReturnDto
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
    }
}

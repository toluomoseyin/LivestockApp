using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LiveStockPhotoDto
    {
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}

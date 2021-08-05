using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class MarketDto
    {
        public string Id { get; set; } = string.Empty;

        public string MarketName { get; set; } = string.Empty;

        public MarketAddressDto Address { get; set; }
        public MarketDto()
        {
            Address = new MarketAddressDto();
        }
    }
}

using System;
using System.Collections.Generic;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LocationMarketDto
    {
        public string location { get; set; }

        public IEnumerable<MarketDataDto> MarketData { get; set; }
    }

    public class MarketDataDto
    {
        public string MarketName { get; set; }

        public int NoOfLivestocks { get; set; }
    }
}

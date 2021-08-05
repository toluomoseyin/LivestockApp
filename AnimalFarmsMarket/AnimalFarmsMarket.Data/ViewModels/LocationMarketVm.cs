using System;
using System.Collections.Generic;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class LocationMarketVm
    {
        public string location { get; set; }

        public IEnumerable<MarketDataVM> MarketData { get; set; }
    }

    public class MarketDataVM
    {
        public string MarketName { get; set; }

        public int NoOfLivestocks { get; set; }
    }
}

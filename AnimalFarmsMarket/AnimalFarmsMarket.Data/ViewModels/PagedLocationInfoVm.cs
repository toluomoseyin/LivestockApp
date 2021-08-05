using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class PagedLocationInfoVm
    {
        public IEnumerable<LocationMarketVm> ResponseData { get; set; } = new List<LocationMarketVm>();
    }
}

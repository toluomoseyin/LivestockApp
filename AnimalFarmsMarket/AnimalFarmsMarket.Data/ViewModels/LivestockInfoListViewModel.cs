using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class LivestockInfoListViewModel
    {
        public PagedLivestockInfoVm Data { get; set; } = new PagedLivestockInfoVm();
        public IEnumerable<LocationMarketVm> Info { get; set; } = new List<LocationMarketVm>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class LivestockMarketVm
    {
        public string MarketName { get; set; }

        public string AgentName { get; set; }
        public List<LivestockViewModel> Livestocks { get; set; }

    }
}

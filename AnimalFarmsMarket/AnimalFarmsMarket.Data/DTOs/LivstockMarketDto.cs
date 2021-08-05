using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivstockMarketDto
    {
        public string MarketName { get; set; }

        public string AgentName { get; set; }
        public List<LivestockToReturnDto> Livestocks { get; set; }

    }
}

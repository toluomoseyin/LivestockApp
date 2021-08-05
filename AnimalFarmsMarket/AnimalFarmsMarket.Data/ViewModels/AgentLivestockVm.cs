using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AgentLivestockVm
    {
        public string Id { get; set; }
        public UserVm User { get; set; }
        public MarketNameVm Market { get; set; }
        public ICollection<LivestockImageVm> Images { get; set; }
        public string Breed { get; set; }

        public int Sex { get; set; }

        public string color { get; set; }

        public string SellingPrice { get; set; }
        public string PurchasePrice { get; set; }

        public int Quantity { get; set; }
        public Decimal Discount { get; set; }

        public bool Availability { get; set; }
    }
}

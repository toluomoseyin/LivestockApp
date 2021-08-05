using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class AgentLivestockDto
    {
        public string Id { get; set; }
        public UserToReturnDto AppUser { get; set; }
        public MarketDto Market { get; set; }
        public ICollection<LivestockImagesToReturnDto> Images { get; set; }
        public string  Breed { get; set; }

        public int Sex { get; set; }

        public string color { get; set; }

        public string SellingPrice { get; set; }
        public string PurchasePrice { get; set; }

        public int Quantity { get; set; }
        public Decimal Discount { get; set; }

        public bool Availability { get; set; }
    }
}

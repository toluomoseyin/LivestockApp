using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class GetLiveStocksDto
    {
        public string Id { get; set; }
        public decimal SellingPrice { get; set; }
        public List<LivestockImagesToReturnDto> LivestockImages { get; set; } = new List<LivestockImagesToReturnDto>();
    }
}

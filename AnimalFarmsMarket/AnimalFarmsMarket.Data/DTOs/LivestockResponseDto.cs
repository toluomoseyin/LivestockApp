using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivestockResponseDto
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public decimal Weight { get; set; }

        public byte Sex { get; set; }

        public string Breed { get; set; }

        public string Color { get; set; }

        public int Age { get; set; }

        public bool Availability { get; set; }

        public string CategoryId { get; set; }

        public UserDto LivestockAgent { get; set; }
         
        public IEnumerable<ReviewDto> Reviews { get; set; }

        public decimal Rating { get; set; }

        public IEnumerable<LivestockImagesToReturnDto> Images { get; set; }

        public MarketDto LivestockMarket { get; set; }

        public LivestockResponseDto()
        {
            Reviews = new HashSet<ReviewDto>();
            Images = new HashSet<LivestockImagesToReturnDto>();
            LivestockMarket = new MarketDto();
        }
    }
}

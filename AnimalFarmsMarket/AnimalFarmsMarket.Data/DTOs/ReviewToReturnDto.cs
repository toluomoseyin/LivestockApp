using System;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ReviewToReturnDto
    {
        public string Id { get; set; }
        public string ReviewText { get; set; }
        public string UserId { get; set; }
        public string LiveStockId { get; set; }
        public string DateCreated { get; set; }
    }
}
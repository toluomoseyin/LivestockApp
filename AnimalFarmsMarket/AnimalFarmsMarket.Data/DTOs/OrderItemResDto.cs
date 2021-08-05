using System;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderItemResDto
    {
        public string Id { get; set; }
        public OrderLivestockResDto Livestock { get; set; }
        public int Quantity { get; set; }
        public string DateCreated { get; set; }
        public decimal Total
        {
            get { return Quantity * Livestock.SellingPrice; }
           
        }

    }
}

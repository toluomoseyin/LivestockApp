using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderItemDto
    {
        public string OrderId { get; set; }

        public string LivestockId { get; set; }

        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.Models
{
    public class OrdersbyStatusDto
    {
        public string Id { get; set; }
        public byte Status { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<OrderItemStatusDto> OrderItems { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.Models
{
    public class ShapedListOfOrderItem
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public string OrderItemId { get; set; }
        public byte Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string recipient { get; set; }
        public byte AgentPaid { get; set; }

    }
}

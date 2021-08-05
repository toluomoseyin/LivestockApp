using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderToreturnByOrderIdDto
    {
        public string Id { get; set; }
        public OrderUserResDto Customer { get; set; }
        public byte Status { get; set; }

        public string TrackingNumber { get; set; }

        public DeliveryModeDto DeliveryMode { get; set; }
        public ShippingDto ShippingPlan { get; set; }
        public decimal PaymentAmount { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public byte PaymentStatus { get; set; }

        public string ShippedTo { get; set; }

        public string DeliveryDate { get; set; }

        public ICollection<OrderItemResDto> OrderItems { get; set; }


    }
}

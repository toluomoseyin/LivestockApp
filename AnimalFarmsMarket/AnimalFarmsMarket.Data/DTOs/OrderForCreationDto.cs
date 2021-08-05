using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderForCreationDto
    {
        public string UserId { get; set; }

        public string DeliveryModeId { get; set; }

        public string ShippingPlanId { get; set; }

        public string PaymentMethodId { get; set; }

        public string ShippedTo { get; set; }

        public int DeliveryPeriod { get; set; }
        public byte PaymentStatus { get; set; }

        public List<OrderItems> OrderItems { get; set; }
    }
}

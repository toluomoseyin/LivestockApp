using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ShippingDetailsDto
    {
        public IEnumerable<DeliveryMode> DeliveryModes { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<ShippingPlan> ShippingPlans { get; set; }
        public ShipDetailsUserDto User { get; set; }
    }
}

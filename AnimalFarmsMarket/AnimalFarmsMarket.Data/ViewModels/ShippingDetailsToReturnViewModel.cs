using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class ShippingDetailsToReturnViewModel
    {
        public string UserId { get; set; }
        public string DeliveryModeId { get; set; }
        public int DeliveryPeriod { get; set; }
        public decimal DeliveryCost { get; set; }
        public string ShippedTo { get; set; }
        public string ShippingPlanId { get; set; }
        public string PaymentMethodId { get; set; }

        public List<OrderItemsVM> OrderItems { get; set; }
    }
}

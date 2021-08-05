using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class ShippingViewModel
    {
        public string Coverage { get; set; }
        public int DeliveryPeriod { get; set; }
        public decimal DeliveryCost { get; set; }
    }
}

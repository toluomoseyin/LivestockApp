using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class ShippingPlanViewModel
    {
        public string Id { get; set; }

        public string Coverage { get; set; }

        public int DeliveryPeriod { get; set; }

        public decimal DeliveryCost { get; set; }
    }
}

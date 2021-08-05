using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class OrdersViewModel
    {
        public string Id { get; set; }

        public byte Status { get; set; }

        public string TrackingNumber { get; set; }

        public byte PaymentStatus { get; set; }

        public string DateUpdated { get; set; }
        
        public string DateCreated { get; set; }
    }
}

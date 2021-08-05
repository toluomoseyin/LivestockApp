using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class InvoiceViewModel
    {
        public string Id { get; set; }
        public InvoiceUserViewModel Customer { get; set; }
        public byte Status { get; set; }

        public string TrackingNumber { get; set; }

        public DeliveryModeViewModel DeliveryMode { get; set; }
        public ShippingViewModel ShippingPlan { get; set; }
        public PaymentMethodViewModel PaymentMethod { get; set; }
        public byte PaymentStatus { get; set; } 

        public string ShippedTo { get; set; }

        public string DeliveryDate { get; set; }

        public ICollection<InvoiceOrderItemViewModel> OrderItems { get; set; }
    }
}

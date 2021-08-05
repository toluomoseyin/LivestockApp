using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string UserId { get; set; }

        public UserViewModel User { get; set; }
        public string Id { get; set; }
        public byte Status { get; set; }

        public string TrackingNumber { get; set; }

        public string DeliveryModeId { get; set; }

        public string ShippingPlanId { get; set; }

        public string PaymentMethodId { get; set; }

        public decimal PaymentAmount { get; set; }

        public byte PaymentStatus { get; set; }

        public string ShippedTo { get; set; }

        public int Period { get; set; }

        public string DeliveryDate { get; set; } 
        public string DateCreated { get; set; }

      

    }
}

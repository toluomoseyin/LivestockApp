using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderToAddDto
    {
        [Column("CustomerId")]
        public string UserId { get; set; }

        public byte Status { get; set; }

        public string TrackingNumber { get; set; }

        public string DeliveryModeId { get; set; }

        public string ShippingPlanId { get; set; }

        public string PaymentMethodId { get; set; }

        public decimal PaymentAmount { get; set; }

        public byte PaymentStatus { get; set; }

        public string ShippedTo { get; set; }

        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

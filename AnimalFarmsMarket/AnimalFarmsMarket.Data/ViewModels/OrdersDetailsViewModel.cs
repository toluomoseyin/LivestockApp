using AnimalFarmsMarket.Data.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class OrdersDetailsViewModel
    {
        public string TrackingNumber { get; set; }
        public decimal PaymentAmount { get; set; }
        public byte PaymentStatus { get; set; }
        public string ShippedTo { get; set; }
        public string DeliveryDate { get; set; }
        public byte Status { get; set; }

        public ICollection<OrderItemResDto> OrderItems { get; set; } = new Collection<OrderItemResDto>();


    }
}


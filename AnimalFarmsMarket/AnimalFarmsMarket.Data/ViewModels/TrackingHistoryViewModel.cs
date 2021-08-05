using AnimalFarmsMarket.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class TrackingHistoryViewModel
    {
        public string Id { get; set; }
        public TrackingHistoryStatus Status { get; set; }

        public string Location { get; set; }
        public string OrderId { get; set; }

        public string CustomerName { get; set; }

        public string DeliveryPersonName { get; set; }

        public string Description { get; set; }

    }
}

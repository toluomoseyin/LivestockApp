using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class TrackingHistoryForUsersDto
    {
        public string Id { get; set; }

        public byte Status { get; set; }

        public string Location { get; set; }

        public string OrderId { get; set; }

        public string CustomerName { get; set; }

        public string DeliveryPersonName { get; set; }

        public string Description { get; set; }

    }
}

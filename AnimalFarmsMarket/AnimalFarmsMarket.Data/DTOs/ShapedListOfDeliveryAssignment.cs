using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
     public  class ShapedListOfDeliveryAssignment
     {
        public string Id { get; set; }
        public string Status { get; set; }
        public string OrderId { get; set; }
        public string DeliveryPersonId { get; set; }
        public string TrackNumber { get; set; }

    }
}

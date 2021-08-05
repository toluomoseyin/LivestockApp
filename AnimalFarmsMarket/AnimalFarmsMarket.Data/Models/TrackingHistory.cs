using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class TrackingHistory : BaseEntity
    {
        [Required]
        public byte Status { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public string DeliveryPersonId { get; set; }

        public DeliveryPerson DeliveryPerson { get; set; }
    }
}

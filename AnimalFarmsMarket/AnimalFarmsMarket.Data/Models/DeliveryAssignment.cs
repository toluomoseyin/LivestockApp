using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class DeliveryAssignment:BaseEntity
    {
        [Required]
        public byte Status { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string DeliveryPersonId { get; set; }
        public DeliveryPerson DeliveryPerson { get; set; }

    }
}

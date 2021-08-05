using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalFarmsMarket.Data.Models
{
    public class ShippingPlan: BaseEntity
    {

        [Required] 
        public string Coverage { get; set; }

        [Required]
        public int DeliveryPeriod { get; set; }

        [Required]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal DeliveryCost { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ShippingPlan()
        {
            Orders = new HashSet<Order>();
        }
    }
}

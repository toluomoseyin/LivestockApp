using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class DeliveryMode : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public ICollection<Order> Orders { get; set; }

        public DeliveryMode()
        {
            Orders = new HashSet<Order>();
        }
    }
}

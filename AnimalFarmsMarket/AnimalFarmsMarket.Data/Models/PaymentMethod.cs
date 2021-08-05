using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class PaymentMethod : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public ICollection<Order> Orders { get; set; }

        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
        }
    }
}

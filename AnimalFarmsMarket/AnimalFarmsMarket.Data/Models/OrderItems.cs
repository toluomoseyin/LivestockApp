using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class OrderItems : BaseEntity
    {
        public string OrderId { get; set; }

        public Order Order { get; set; }

        public string LivestockId { get; set; }

        public Livestock Livestock { get; set; }

        [Required]
        public int Quantity { get; set; }
        public byte AgentPaid { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalFarmsMarket.Data.Models
{
    public class Order : BaseEntity
    {
        [Column("CustomerId")]
        public string UserId { get; set; }

        public AppUser User { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public string TrackingNumber { get; set; }

        public string DeliveryModeId { get; set; }

        public DeliveryMode DeliveryMode { get; set; }

        public string ShippingPlanId { get; set; }

        public ShippingPlan ShippingPlan { get; set; }

        public string PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryAssignment DeliveryAssignment { get; set; }

        [Required]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal PaymentAmount { get; set; }

        [Required]
        public byte PaymentStatus { get; set; }

        [Required]
        [StringLength(125)]
        public string ShippedTo { get; set; }

        [Required]
        [StringLength(50)]
        public string DeliveryDate { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }

        public ICollection<TrackingHistory> TrackingHistories { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItems>();
            TrackingHistories = new List<TrackingHistory>();
        }
    }
}

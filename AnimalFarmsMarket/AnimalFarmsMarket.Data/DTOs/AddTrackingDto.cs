using System.ComponentModel.DataAnnotations;


namespace AnimalFarmsMarket.Data.DTOs
{
   public class AddTrackingDto
    {
        [Required]
        public byte Status { get; set; }

        [Required]
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public string DeliveryPersonId { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

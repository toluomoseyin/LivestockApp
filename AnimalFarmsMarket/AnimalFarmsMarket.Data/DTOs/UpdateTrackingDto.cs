using System.ComponentModel.DataAnnotations;


namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateTrackingDto
    {
        [Required]
        public byte Status { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

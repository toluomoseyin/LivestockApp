using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class LivestockImages : BaseEntity
    {
        [Required]
        public string ImageUrl { get; set; }

        public string PublicId { get; set; }

        public bool IsMain { get; set; }

        public string LivestockId { get; set; }

        public Livestock Livestock { get; set; }
    }
}

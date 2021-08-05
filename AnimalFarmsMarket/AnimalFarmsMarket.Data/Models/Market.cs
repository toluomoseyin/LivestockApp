using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Market : BaseEntity
    {
        [Required]
        [MaxLength(125, ErrorMessage = "Market Name can not be more than 125 characters")]
        public string MarketName { get; set; }

        public MarketAddress MarketAddress { get; set; }

        public ICollection<Livestock> Livestocks { get; set; }

        public Market()
        {
            Livestocks = new HashSet<Livestock>();
        }
    }
}
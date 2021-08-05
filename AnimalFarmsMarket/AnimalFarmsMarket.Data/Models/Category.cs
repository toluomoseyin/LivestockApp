using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(125)]
        public string Name { get; set; }

        
        public string Description { get; set; }

        public ICollection<Livestock> Livestocks { get; set; }

        public Category()
        {
            Livestocks = new HashSet<Livestock>();
        }
    }
}

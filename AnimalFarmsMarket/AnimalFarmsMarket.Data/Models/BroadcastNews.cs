using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class BroadcastNews:BaseEntity
    {
        [Required]
        [StringLength(125, ErrorMessage = "Title of broadcast is more than the required length")]
        public string Title { get; set; }

        [Required]
        public string Article { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Thumbnail { get; set; }
        public string PublicId { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public AppUser User { get; set; }
        public string UserId { get; set; }

        public BroadcastNews()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
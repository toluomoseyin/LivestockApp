using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        [StringLength(125, ErrorMessage = "Comment is more than the maximum required length")]
        public string CommentText { get; set; }

        public string BroadCastNewsId { get; set; }

        public BroadcastNews BroadcastNews { get; set; }

        public AppUser User { get; set; }

        public string UserId { get; set; }
    }
}

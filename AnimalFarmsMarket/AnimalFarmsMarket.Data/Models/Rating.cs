namespace AnimalFarmsMarket.Data.Models
{
    public class Rating: BaseEntity
    {
        public AppUser User { get; set; }
        public string UserId { get; set; }

        public Livestock Livestock { get; set; }
        public string LivestockId { get; set; }
        
        public int RatingFigure { get; set; }

    }
}
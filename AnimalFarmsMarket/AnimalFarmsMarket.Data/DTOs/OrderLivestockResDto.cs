namespace AnimalFarmsMarket.Data.DTOs
{
    public class OrderLivestockResDto
    {
        public string Id { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Weight { get; set; }

        public byte Sex { get; set; }
        public string Breed { get; set; }
    }
}
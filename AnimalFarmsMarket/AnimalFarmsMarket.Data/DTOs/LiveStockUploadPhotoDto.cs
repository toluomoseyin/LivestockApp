using Microsoft.AspNetCore.Http;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LiveStockUploadPhotoDto
    {
        public string LiveStockId { get; set; }
        public IFormFile LivesStockPhoto { get; set; }
    }
}

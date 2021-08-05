
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AddLivestockViewModel
    {
        [Required(ErrorMessage = "Description field is required")]
        [MaxLength(250, ErrorMessage ="Maximum Length for Description field is 250 character")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        [DisplayName("Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Selling price is required")]
        [DisplayName("Selling Price")]

        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Discount is required")]
       
        public decimal Discount { get; set; }

        [Range(1, 200, ErrorMessage = "Quantity should be between 1 and 200")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        
        public decimal Weight { get; set; }

        [Required(ErrorMessage ="Sex is required")]
        public byte Sex { get; set; }

        [Required(ErrorMessage = "Breed is required")]
        public string Breed { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Range(1, 30, ErrorMessage = "Age should be between 1 and 30")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public bool Availability { get; set; }

        [Required(ErrorMessage = "Market field is required")]
        [DisplayName("Market")]
        public string MarketId { get; set; }

        [Required(ErrorMessage = "Category field is required")]
        [DisplayName("Category")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Agent field is required")]
        public string AgentId { get; set; }

        public string LivestockId { get; set; }

        public IFormFile LivestockPhoto { get; set; }


    }
}

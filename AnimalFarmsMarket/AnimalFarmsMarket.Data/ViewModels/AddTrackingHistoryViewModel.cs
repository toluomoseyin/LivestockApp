

using AnimalFarmsMarket.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AddTrackingHistoryViewModel
    {

        public string UserId { get; set; }

        public string  OrderId { get; set; }

        [Required(ErrorMessage ="Location is Required")]
        public States Location { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public byte Status { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

    }
}

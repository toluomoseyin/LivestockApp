using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Delivery Address is required")]
        public AddressViewModel DeliveryAddress { get; set; }
    }
}
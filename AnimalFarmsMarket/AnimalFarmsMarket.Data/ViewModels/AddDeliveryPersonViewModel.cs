using AnimalFarmsMarket.Data.Enum;
using System.ComponentModel.DataAnnotations;


namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AddDeliveryPersonViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password do not Match")]
        public string ConfirmPassword { get; set; }

        public byte Gender { get; set; }
        public string ZipCode { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public Countries Country { get; set; }


        [Required(ErrorMessage = "NIN is Required")]
        public string NIN { get; set; }

        [Required(ErrorMessage = "NINT Tracking Id is Required")]
        public string NINTrackingId { get; set; }

        public Coverage Coverage { get; set; }

        [Required]
        public string CoverageLocation { get; set; }

        
    }
}

using AnimalFarmsMarket.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AddAgentViewModel
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
        public AddressViewModel Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }

        public byte Gender { get; set; }

        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Business Location is Required")]
        public string BusinessLocation { get; set; }

        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid NIN")]
        [Required(ErrorMessage = "NIN is Required")]
        public string NIN { get; set; }

        [Required(ErrorMessage = "NINT tracking Id is Required")]
        public string NINTrackingId { get; set; }

        public Banks Bank { get; set; }

        [Required(ErrorMessage = "Account name is Required")]
        public string AccountName { get; set; }


        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong account format")]
        [Required(ErrorMessage = "Account number is Required")]
        public string AccountNumber { get; set; }


    }
}

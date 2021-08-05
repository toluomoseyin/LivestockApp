using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class RegisterAgentDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender field is Required")]
        public byte Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", ErrorMessage = "Password must contain at least one uppercase, lowercase, number and special symbol")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        [MaxLength(100, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string UserName { get; set; }

        [MaxLength(150, ErrorMessage = "Zip Code can not be more than 150 characters")]
        public string ZipCode { get; set; }

        public AddressToReturnDto Address { get; set; }

        public string BusinessLocation { get; set; }

        public string NIN { get; set; }

        public string NINTrackingId { get; set; }

        public string Bank { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }
    }
}

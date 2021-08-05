using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string ZipCode { get; set; }
    }
}

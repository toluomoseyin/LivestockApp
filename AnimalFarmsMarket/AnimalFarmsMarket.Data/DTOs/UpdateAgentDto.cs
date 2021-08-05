using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateAgentDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }
        public byte Gender { get; set; }

        [MaxLength(50, ErrorMessage = "Zip Code can not be more than 50 characters")]
        public string ZipCode { get; set; }

        public AddressToReturnDto Address { get; set; }

        public string BusinessLocation { get; set; }
    }
}

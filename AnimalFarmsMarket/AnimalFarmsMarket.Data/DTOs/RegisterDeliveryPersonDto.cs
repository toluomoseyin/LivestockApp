using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class RegisterDeliveryPersonDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }


        [Required(ErrorMessage = "Gender is Required")]
        public byte Gender { get; set; }
        public bool IsActive { get; set; }

        public string Photo { get; set; }

        [MaxLength(50, ErrorMessage = "Zip Code can not be more than 50 characters")]
        public string ZipCode { get; set; }

        public string PublicId { get; set; }

        public string Coverage { get; set; }
        public string CoverageLocation { get; set; }
        [Required]
        public string NIN { get; set; }
        [Required]
        public string NINTrackingId { get; set; }

        public string Street { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}

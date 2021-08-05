using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateDeliveryPersonDto
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        public byte Gender { get; set; }

        public string ZipCode { get; set; }

        public string PublicId { get; set; }

        public string Coverage { get; set; }
        public string CoverageLocation { get; set; }


        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}

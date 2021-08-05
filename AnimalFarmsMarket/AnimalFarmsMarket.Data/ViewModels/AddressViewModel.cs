using AnimalFarmsMarket.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class AddressViewModel
    {
        public string Street { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public Countries Country { get; set; }
    }
}

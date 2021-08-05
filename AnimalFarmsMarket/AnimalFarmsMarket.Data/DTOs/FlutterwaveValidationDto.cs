using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class FlutterwaveValidationDto
    {
        public string otp { get; set; }
        public string flw_ref { get; set; }
        public string type { get; set; } = "card";
    }
}

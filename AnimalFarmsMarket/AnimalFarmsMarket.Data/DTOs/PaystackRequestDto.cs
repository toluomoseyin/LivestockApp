using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackRequestDto
    {
        public string email { get; set; }
        public string amount { get; set; }
        public string callback_url { get; set; }
    }
}

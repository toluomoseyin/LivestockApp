using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackResolveResponseDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public PaystackData data { get; set; }
    }

    public class PaystackData
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_id { get; set; }
    }
}

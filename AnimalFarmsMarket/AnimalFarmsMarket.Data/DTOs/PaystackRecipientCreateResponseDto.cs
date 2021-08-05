using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackRecipientCreateResponseDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public PaystackRecipientData data { get; set; }
    }

    public class PaystackRecipientData
    {
        public bool active { get; set; }
        public string createdAt { get; set; }
        public string currency { get; set; }
        public int id { get; set; }
        public int integration { get; set; }
        public string name { get; set; }
        public string recipient_code { get; set; }
        public string nuban { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public string authorization_code { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_name { get; set; }

    }
}

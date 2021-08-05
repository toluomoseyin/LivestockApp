using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackInitiateAgentTransferResponseDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public PaystackInitiateData data { get; set; }
    }

    public class PaystackInitiateData
    {
        public decimal amount { get; set; }
        public int integration { get; set; }
        public string domain { get; set; }
        public string currency { get; set; }
        public string source { get; set; }
        public string reason { get; set; }
        public int recipient { get; set; }
        public string status { get; set; }
        public string transfer_code { get; set; }
        public int id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }


    }
}

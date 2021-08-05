using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackInitiateAgentTransferRequestDto
    {
        public string source { get; set; }
        public string reason { get; set; }
        public decimal amount { get; set; }
        public string recipient { get; set; }
    }
}

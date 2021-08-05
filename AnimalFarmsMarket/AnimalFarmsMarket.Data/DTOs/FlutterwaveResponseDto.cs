using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class FlutterwaveResponseDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public FlutterData data { get; set; }
    }
    public class FlutterData
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        public string flw_ref { get; set; }
        public decimal amount { get; set; }
        public decimal charged_amount { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackBanksCodeDto
    {
        public string status { get; set; }
        public string message { get; set; }
        public IEnumerable<data> data { get; set; }
    }

    public class data
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

}

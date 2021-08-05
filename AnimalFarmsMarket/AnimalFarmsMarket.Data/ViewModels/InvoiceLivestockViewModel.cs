using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class InvoiceLivestockViewModel
    {
        public string Id { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Weight { get; set; }

        public byte Sex { get; set; }
        public string Breed { get; set; }
    }
}

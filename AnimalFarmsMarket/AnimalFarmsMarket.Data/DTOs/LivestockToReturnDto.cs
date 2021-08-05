using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LivestockToReturnDto
    {
        public string Id { get; set; }

        public decimal SellingPrice { get; set; }


        public string Description { get; set; }

        public decimal PurchasePrice { get; set; }



        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public decimal Weight { get; set; }

        public byte Sex { get; set; }

        public string Breed { get; set; }

        public string Color { get; set; }

        public int Age { get; set; }

        public bool Availability { get; set; }

        public string CategoryId { get; set; }

        public IEnumerable<LivestockImages> Images { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class GetOrderDto
    {
        public string Id { get; set; }
        public byte Status { get; set; }
        public string CreatedAt { get; set; }
        public string Breed { get; set; }
        public bool Availability { get; set; }
    }
}

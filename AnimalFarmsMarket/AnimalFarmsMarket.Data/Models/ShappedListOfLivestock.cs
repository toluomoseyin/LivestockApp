using System;

namespace AnimalFarmsMarket.Data.Models
{
    public class ShappedListOfLivestock
    {
        public string Id { get; set; }

        public decimal Price { get; set; }

        public string Photo { get; set; }

        public string AgentId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string Breed { get; set; }

        public string Location { get; set; }

        public bool Availability { get; set; }
    }
}

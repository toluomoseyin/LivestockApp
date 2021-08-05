using System;

namespace AnimalFarmsMarket.Data.Models
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DateCreated { get; set; } = DateTime.Now.ToString();
        public string DateUpdated { get; set; } = DateTime.Now.ToString();
    }
}

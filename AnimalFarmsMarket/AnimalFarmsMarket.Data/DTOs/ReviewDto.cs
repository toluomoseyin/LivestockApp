using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ReviewDto
    {
        public string Id { get; set; }

        public string ReviewText { get; set; }

        public UserDto User { get; set; }
    }
}

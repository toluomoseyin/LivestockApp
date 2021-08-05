using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class AddAssignmentDto
    {

        [Required]
        public string OrderId { get; set; }


    }
}

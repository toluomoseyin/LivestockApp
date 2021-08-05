using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateAssignmentDto
    {
        [Required]
        public byte Status { get; set; }
        [Required]
        public string DeliveryPersonId { get; set; }

    }
}

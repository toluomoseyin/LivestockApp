using System;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ConfirmEmailDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}

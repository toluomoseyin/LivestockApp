using System;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
    }
}

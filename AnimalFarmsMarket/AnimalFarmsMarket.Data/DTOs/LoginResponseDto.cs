using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}

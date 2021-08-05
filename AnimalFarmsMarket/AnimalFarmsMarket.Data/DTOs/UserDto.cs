using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string BusinessLocation { get; set; }
        public IList<string> Roles { get; set; }

    }
}

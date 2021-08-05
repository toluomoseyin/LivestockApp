using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class UpdateUserPhotoDto
    {
        public IFormFile Photo { get; set; }
    }
}

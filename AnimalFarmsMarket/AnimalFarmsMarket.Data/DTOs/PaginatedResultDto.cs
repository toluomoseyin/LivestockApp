using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaginatedResultDto <T>
    {
        public PageMetaData PageMetaData { get; set; }

        public IEnumerable<T> ResponseData { get; set; } = new List<T>();
    }
}
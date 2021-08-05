using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class PagedLivestockInfoVm
    {
        public PageMetaData PageMetaData { get; set; } = new PageMetaData();
        public IEnumerable<ShapedListOfLivestockVm> ResponseData { get; set; } = new List<ShapedListOfLivestockVm>();
    }
}

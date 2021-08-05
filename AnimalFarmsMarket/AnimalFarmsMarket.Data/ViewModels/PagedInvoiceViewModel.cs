using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class PagedInvoiceViewModel
    {
        public PageMetaData PageMetaData { get; set; }

        public List<InvoiceViewModel> ResponseData { get; set; } = new List<InvoiceViewModel>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class InvoiceOrderItemViewModel
    {
        public string Id { get; set; }
        public InvoiceLivestockViewModel Livestock { get; set; }
        public int Quantity { get; set; }
        public string DateCreated { get; set; }

        public Decimal Total { get; set; }
    }
}

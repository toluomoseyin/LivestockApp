using System.Collections.Generic;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class DashboardViewModel
    {
        public PaginatedResultDto<OrderDetailsViewModel> Payments { get; set; } =
            new PaginatedResultDto<OrderDetailsViewModel>();
        public PagedInvoiceViewModel Invoices { get; set; } = new PagedInvoiceViewModel();

        public PaginatedResultDto<OrdersViewModel> Orders { get; set; } = new PaginatedResultDto<OrdersViewModel>();

        public StatsViewModel Stats { get; set; } = new StatsViewModel();

        public PaginatedResultDto<ShapedListOfOrderItem> AgentOrder { get; set; } =
            new PaginatedResultDto<ShapedListOfOrderItem>();
    }
}
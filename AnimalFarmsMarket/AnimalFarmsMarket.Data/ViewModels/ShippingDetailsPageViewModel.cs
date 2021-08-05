using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class ShippingDetailsPageViewModel
    {
        public List<ShippingPlanViewModel> ShippingPlans { get; set; } = new List<ShippingPlanViewModel>();

        public List<DeliveryModesViewModel> DeliveryModes { get; set; } = new List<DeliveryModesViewModel>();

        public List<PaymentInfoViewModel> PaymentMethods { get; set; } = new List<PaymentInfoViewModel>();

        public UserViewModel User { get; set; }
    }
}
